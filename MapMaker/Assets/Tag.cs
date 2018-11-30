using System;
using System.Collections.Generic;
using System.Text;

namespace MapMaker.Assets
{
    class Tag
    {
        public const byte IntegerType = 0;
        public const byte DoubleType = 1;
        public const byte CharType = 2;
        public const byte BoolType = 3;
        public const byte StringType = 4;
        public const byte ArrayType = 5;
        public const byte ObjectType = 6;

        public readonly byte Type;
        public readonly int IntValue;
        public readonly double DoubleValue;
        public readonly char CharValue;
        public readonly bool BoolValue;
        public readonly string StringValue;
        public readonly Tag[] Array;
        private readonly Dictionary<string, Tag> _children;

        private Tag(int value)
        {
            Type = IntegerType;
            IntValue = value;
        }

        private Tag(double value)
        {
            Type = DoubleType;
            DoubleValue = value;
        }

        private Tag(char value)
        {
            Type = CharType;
            CharValue = value;
        }

        private Tag(bool value)
        {
            Type = BoolType;
            BoolValue = value;
        }

        private Tag(string value)
        {
            Type = StringType;
            StringValue = value;
        }

        private Tag(Tag[] array)
        {
            Type = ArrayType;
            Array = array;
        }

        private Tag(Dictionary<string, Tag> children)
        {
            Type = ObjectType;
            _children = children;
        }

        public Tag this[string childName] => _children[childName];

        public bool ContainsChild(string childName) => _children.ContainsKey(childName);

        public override string ToString() => ToString("");

        private string ToString(string indentation)
        {
            switch (Type)
            {
                case IntegerType: return indentation + IntValue;

                case DoubleType: return indentation + DoubleValue;

                case CharType: return indentation + CharValue;

                case BoolType: return indentation + BoolValue;

                case StringType: return indentation + StringValue;

                case ArrayType:
                {
                    var builder = new StringBuilder();

                    foreach (var t in Array)
                    {
                        if (t.Type == ObjectType)
                        {
                            builder.Append(indentation);
                            builder.Append("object:\n");
                            builder.Append(t.ToString(indentation + "    "));
                        }
                        else
                        {
                            builder.Append(t.ToString() + "\n");
                        }
                    }

                    return builder.ToString();
                }

                case ObjectType:
                {
                    var builder = new StringBuilder();

                    foreach (var pair in _children)
                    {
                        var key = pair.Key;
                        var value = pair.Value;

                        builder.Append(indentation);
                        builder.Append(key);

                        if (value.Type == ObjectType)
                        {
                            builder.Append(":\n");
                            builder.Append(value.ToString(indentation + "    "));
                        }
                        else if (value.Type == ArrayType)
                        {
                            builder.Append("[]\n");
                            builder.Append(value.ToString(indentation + "    "));
                        }
                        else
                        {
                            builder.Append(' ');
                            builder.Append(value.ToString("") + "\n");
                        }
                    }

                    return builder.ToString();
                }
            }

            throw new InvalidOperationException("Tag has an invalid type.");
        }

        public static Tag Load(string filePath)
        {
            var source = AssetLoader.LoadText(filePath);

            var errors = new List<string>();
            var stream = Lex(source, errors);
            var tag = ParseObject(stream, errors, 0);

            if (errors.Count > 0)
            {
                var message = "";

                foreach (var e in errors)
                    message += e + '\n';

                throw new Exception(message);
            }

            return tag;
        }

        private static Tag ParseObject(TokenStream stream, List<string> errors, int indentation)
        {
            var fields = new Dictionary<string, Tag>();

            while (stream.Current.Kind != Token.Eof && stream.Current.Indentation == indentation)
            {
                if (stream.Current.Kind != Token.Identifier)
                {
                    errors.Add($"Line {stream.Line}: expected an identifier.");
                    stream.Next();
                }
                else
                {
                    var name = stream.Current;
                    var tag = (Tag) null;
                    var array = (List<Tag>) null;

                    switch (stream.Next().Kind)
                    {
                        case Token.Colon:
                            stream.Next();
                            tag = ParseObject(stream, errors, indentation + 1);
                            break;

                        case Token.Brackets:
                            stream.Next();
                            array = new List<Tag>();

                            while (stream.Current.Indentation == indentation + 1)
                            {
                                switch (stream.Next().Kind)
                                {
                                    case Token.Object:
                                        if (stream.Next().Kind != Token.Colon)
                                            errors.Add($"Line {stream.Line}: expected ':'.");
                                        else
                                            stream.Next();

                                        array.Add(ParseObject(stream, errors, indentation + 2));
                                        break;

                                    case Token.Integer:
                                        array.Add(new Tag(int.Parse(stream.Current.Value)));
                                        stream.Next();
                                        break;

                                    case Token.Double:
                                        array.Add(new Tag(double.Parse(stream.Current.Value)));
                                        stream.Next();
                                        break;

                                    case Token.String:
                                        array.Add(new Tag(stream.Current.Value));
                                        stream.Next();
                                        break;

                                    case Token.True:
                                        array.Add(new Tag(true));
                                        stream.Next();
                                        break;

                                    case Token.False:
                                        array.Add(new Tag(false));
                                        stream.Next();
                                        break;
                                }
                            }

                            tag = new Tag(array.ToArray());
                            break;

                        case Token.Integer:
                            tag = new Tag(int.Parse(stream.Current.Value));
                            stream.Next();
                            break;

                        case Token.Double:
                            tag = new Tag(double.Parse(stream.Current.Value));
                            stream.Next();
                            break;

                        case Token.String:
                            tag = new Tag(stream.Current.Value);
                            stream.Next();
                            break;

                        case Token.True:
                            tag = new Tag(true);
                            stream.Next();
                            break;

                        case Token.False:
                            tag = new Tag(false);
                            stream.Next();
                            break;

                        case Token.Identifier:
                            errors.Add($"Line {stream.Line}: expected ':'.");
                            tag = ParseObject(stream, errors, indentation + 1);
                            break;

                        case Token.Object:
                            errors.Add($"Line {stream.Line}: cannot create unnamed object inside an object.");

                            if (stream.Current.Kind == Token.Colon)
                                stream.Next();

                            ParseObject(stream, errors, indentation + 1);
                            break;
                    }

                    fields.Add(name.Value, tag);
                }
            }

            return new Tag(fields);
        }

        private static TokenStream Lex(string source, List<string> errors)
        {
            var tokens = new List<Token>();
            var index = 0;
            var line = 1;
            var indentation = 0;

            while (index < source.Length)
            {
                var value = "";
                var isDouble = false;

                switch (source[index])
                {
                    case '\n':
                        ++index;
                        ++line;
                        indentation = 0;
                        break;

                    case '\t':
                        ++index;
                        ++indentation;
                        break;

                    case ' ':
                    case '\r':
                        ++index;
                        break;

                    case '[':
                        ++index;

                        if (source[index] == ']')
                            ++index;
                        else
                            errors.Add($"Line {line}: expected ']'.");

                        tokens.Add(new Token(line, indentation, Token.Brackets, null));
                        break;

                    case ':':
                        ++index;
                        tokens.Add(new Token(line, indentation, Token.Colon, null));
                        break;

                    case '-':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        value += source[index];

                        for (var c = source[++index]; IsDigit(c); c = source[++index])
                            value += c;

                        if (source[index] == '.')
                        {
                            ++index;
                            value += '.';
                            isDouble = true;

                            for (var c = source[index]; IsDigit(c); c = source[++index])
                                value += c;
                        }

                        tokens.Add(new Token(line, indentation, isDouble ? Token.Double : Token.Integer, value));
                        break;

                    case '_':
                    case 'A': case 'B': case 'C': case 'D': case 'E': case 'F': case 'G': case 'H':
                    case 'I': case 'J': case 'K': case 'L': case 'M': case 'N': case 'O': case 'P':
                    case 'Q': case 'R': case 'S': case 'T': case 'U': case 'V': case 'W': case 'X':
                    case 'Y': case 'Z':
                    case 'a': case 'b': case 'c': case 'd': case 'e': case 'f': case 'g': case 'h':
                    case 'i': case 'j': case 'k': case 'l': case 'm': case 'n': case 'o': case 'p':
                    case 'q': case 'r': case 's': case 't': case 'u': case 'v': case 'w': case 'x':
                    case 'y': case 'z':
                        for (var c = source[index]; IsIdentifier(c); c = source[++index])
                            value += c;

                        if (value == "object")
                            tokens.Add(new Token(line, indentation, Token.Object, null));
                        else if (value == "true")
                            tokens.Add(new Token(line, indentation, Token.True, null));
                        else if (value == "false")
                            tokens.Add(new Token(line, indentation, Token.False, null));
                        else
                            tokens.Add(new Token(line, indentation, Token.Identifier, value));

                        break;

                    case '"':
                        ++index;

                        for (var c = source[index]; c != '"'; c = source[++index])
                            value += c;

                        ++index;
                        tokens.Add(new Token(line, indentation, Token.String, value));
                        break;

                    default:
                        errors.Add($"Line {line}: unexpected character '{source[index]}'.");
                        ++index;
                        break;
                }
            }

            tokens.Add(new Token(++line, 0, Token.Eof, null));
            return new TokenStream(tokens);
        }

        private static bool IsDigit(char c) =>
            c >= '0' && c <= '9';

        private static bool IsIdentifier(char c) =>
            (c == '_') ||
            (c >= 'A' && c <= 'Z') ||
            (c >= 'a' && c <= 'z') ||
            (c >= '0' && c <= '9');

        private sealed class TokenStream
        {
            private readonly List<Token> _tokens;
            private int _index;

            public TokenStream(List<Token> tokens)
            {
                _tokens = tokens;
            }

            public int Line => _tokens[_index].Line;

            public Token Current => _tokens[_index];

            public Token Next() => _tokens[++_index];
        }

        private sealed class Token
        {
            public const int Eof = 0;
            public const int Identifier = 1;
            public const int Integer = 2;
            public const int Double = 3;
            public const int String = 4;
            public const int Brackets = 5;
            public const int Colon = 6;
            public const int Object = 7;
            public const int True = 8;
            public const int False = 9;

            public readonly int Line;
            public readonly int Indentation;
            public readonly int Kind;
            public readonly string Value;

            public Token(int line, int indentation, int kind, string value)
            {
                Line = line;
                Indentation = indentation;
                Kind = kind;
                Value = value;
            }
        }
    }
}