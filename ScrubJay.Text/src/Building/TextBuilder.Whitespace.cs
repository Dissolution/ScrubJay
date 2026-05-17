namespace ScrubJay.Text.Building;

partial class TextBuilder
{
    private Whitespace? _whitespace;

    public string CurrentNewLine
    {
        get
        {
            if (_whitespace is null)
            {
                return WhitespaceManager.DefaultNewLine;
            }
            else
            {
                return _whitespace.FullNewLineString;
            }
        }
        set
        {
            _whitespace ??= new();
            _whitespace.CurrentNewLine = value;
        }
    }

    public string CurrentDefaultIndent
    {
        get
        {
            if (_whitespace is null)
            {
                return WhitespaceManager.DefaultIndent;
            }
            else
            {
                return _whitespace.CurrentDefaultIndent;
            }
        }
        set
        {
            _whitespace ??= new();
            _whitespace.CurrentDefaultIndent = value;
        }
    }

    internal bool IndentAware => _whitespace is not null && _whitespace.IndentCount > 0;

#region NewLine
    public TextBuilder NewLine() => Append(CurrentNewLine);

    public TextBuilder NewLines(int count) => Repeat(count, CurrentNewLine);
#endregion

#region Indents
    public TextBuilder Indent(string? indent = null)
    {
        _whitespace ??= new();
        _whitespace.AddIndent(indent);
        return this;
    }

    public TextBuilder Outdent(bool throwIfNotPossible = false)
    {
        if (_whitespace is null)
        {
            if (throwIfNotPossible)
                throw new InvalidOperationException("Cannot Outdent: No Indents have been added");

        }
        else
        {
            var removed = _whitespace.TryRemoveIndent();
            if (!removed && throwIfNotPossible)
                throw new InvalidOperationException("Cannot Outdent: No Indents remain");
        }
        return this;
    }

    public TextBuilder Indented(Action<TextBuilder>? build)
    {
        if (build is not null)
        {
            _whitespace ??= new();
            _whitespace.AddIndent();
            build(this);
            _whitespace.TryRemoveIndent();
        }
        return this;
    }

    public IDisposable TemporaryIndent(string? indent = null)
    {
        var disposable = new DisposableTBA(this, static tb => tb.Outdent());
        Indent(indent);
        return disposable;
    }
#endregion

#region Blocks
    public TextBuilder BracesBlock(Action<TextBuilder>? indentedBlock)
    {
        if (indentedBlock is null)
        {
            return AppendLine(" { }");
        }

        _whitespace ??= new();

        return this
            .If(!IsStartLine(), TB.NewLine)
            .Append('{')
            .Indent()
            .NewLine()
            .Invoke(indentedBlock)
            .Outdent()
            .Switch(sw => sw
                .Case(IsStartLine(), _ =>
                {
                    // we need to back off 1 indent before writing the }
                    _whitespace.TryPeekLastIndent(out var lastIndent);
                    _position -= lastIndent!.Length;
                })
                .Case(IsStartOutdentedLine(), TB.None)
                .Default(TB.NewLine))
            .Append('}')
            .NewLine();
    }
#endregion

    internal bool IsStartLine()
    {
        if (_position == 0)
            return true;

        text ending;

        if (_whitespace is null)
        {
            ending = WhitespaceManager.DefaultNewLine;
        }
        else
        {
            ending = _whitespace.FullNewLine;
        }

        return Written.EndsWith(ending, StringComparison.Ordinal);
    }

    internal bool IsStartOutdentedLine()
    {
        if (_position == 0)
            return true;

        text ending;

        if (_whitespace is null)
        {
            ending = WhitespaceManager.DefaultNewLine;
        }
        else
        {
            ending = _whitespace.OutdentNewLine;
        }

        return Written.EndsWith(ending, StringComparison.Ordinal);
    }


    internal void WriteWithSubstituteNewLines(scoped text text)
    {
        Debug.Assert(IndentAware);
        Debug.Assert(_whitespace is not null);

        var currentNewLine = _whitespace!.CurrentNewLine;
        var fullNewLine = _whitespace.FullNewLine;

        if (text.Length < currentNewLine.Length)
        {
            Write(text);
        }
        else if (currentNewLine.Equals(text, StringComparison.Ordinal))
        {
            Write(fullNewLine);
        }
        else
        {
            var lines = text.SplitOn(currentNewLine);
            Delimit(fullNewLine, ref lines, TB.Write);
        }
    }

    internal text GetCurrentPositionIndent()
    {
        var written = this.Written;

        // find the last newline that was written
        int lastNewLineIndex = written.LastIndexOfAny(CurrentNewLine);
        if (lastNewLineIndex == -1)
        {
            // start at 0 then
            lastNewLineIndex = 0;
        }

        // skip ahead by the Current NewLine length
        int start = lastNewLineIndex + CurrentNewLine.Length;

        // we're looking for whitespace
        for (int i = start; i < written.Length; i++)
        {
            if (!char.IsWhiteSpace(written[i]))
            {
                // whatever we found
                return written.Slice(start, i - start);
            }
        }

        // everything was whitespace
        return written.Slice(start);
    }

    internal void IndentAwareInvoke(Action<TextBuilder>? build)
    {
        if (build is not null)
        {
            if (IndentAware)
            {
                // ReSharper disable once NotDisposedResource
                Whitespace? oldWhitespace = Interlocked.Exchange(ref _whitespace, new Whitespace());
                text currentIndent = GetCurrentPositionIndent();
                _whitespace!.AddIndent(currentIndent);
                build(this);
                Whitespace newWhitespace = Interlocked.Exchange(ref _whitespace, oldWhitespace)!;
                newWhitespace.Dispose();
            }
            else
            {
                build(this);
            }
        }
    }


    internal void IndentAwareInvoke<T>(Action<TextBuilder, T>? buildItem, T value)
    {
        if (buildItem is not null)
        {
            if (IndentAware)
            {
                // ReSharper disable once NotDisposedResource
                Whitespace? oldWhitespace = Interlocked.Exchange(ref _whitespace, new Whitespace());
                text currentIndent = GetCurrentPositionIndent();
                _whitespace!.AddIndent(currentIndent);
                buildItem(this, value);
                Whitespace newWhitespace = Interlocked.Exchange(ref _whitespace, oldWhitespace)!;
                newWhitespace.Dispose();
            }
            else
            {
                buildItem(this, value);
            }
        }
    }

    internal void IndentAwareInvoke<T1, T2>(Action<TextBuilder, T1, T2>? buildItem, T1 first, T2 second)
    {
        if (buildItem is null)
            return;
        
        if (IndentAware)
        {
            // ReSharper disable once NotDisposedResource
            Whitespace? oldWhitespace = Interlocked.Exchange(ref _whitespace, new Whitespace());
            text currentIndent = GetCurrentPositionIndent();
            _whitespace!.AddIndent(currentIndent);
            buildItem(this, first, second);
            Whitespace newWhitespace = Interlocked.Exchange(ref _whitespace, oldWhitespace)!;
            newWhitespace.Dispose();
        }
        else
        {
            buildItem(this, first, second);
        }
    }
}