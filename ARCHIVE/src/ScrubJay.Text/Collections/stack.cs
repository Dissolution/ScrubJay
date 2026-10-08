
    using System.Buffers;

    namespace ScrubJay.Text.Collections;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public ref partial struct stack<T> :
        IList<T>, IReadOnlyList<T>,
        ICollection<T>, IReadOnlyCollection<T>,
        IEnumerable<T>,
        IBufferWriter<T>,
        IDisposable
    {
        internal T[]? _rentedArray;
        internal Span<T> _span;
        internal int _position;
        
        T IList<T>.this[int index]
        {
            get => this[index];
            set => this[index] = value;
        }

        T IReadOnlyList<T>.this[int index] => this[index];

        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay => AsSpan().ToString();
        
        public ref T this[int index] => ref _span[index];

        public int Length
        {
            get => _position;
            set => _position = Math.Clamp(value, 0, _span.Length);
        }

        public int Capacity => _span.Length;
        
        /// <summary>Returns the underlying storage of the builder.</summary>
        public Span<T> RawSpan => _span;
        
        public stack()
        {
            _rentedArray = null;
            _span = default;
            _position = 0;
        }

        public stack(Span<T> initialBuffer)
        {
            _rentedArray = null;
            _span = initialBuffer;
            _position = 0;
        }

        public stack(int minCapacity)
        {
            _rentedArray = ArrayPool<T>.Shared.Rent(minCapacity);
            _span = _rentedArray;
            _position = 0;
        }

        public void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > _span.Length)
            {
                Grow(minCapacity - _position);
            }
        }

        
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowPush(T value)
        {
            Grow(1);
            Push(value);
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T value)
        {
            int pos = _position;
            Span<T> span = _span;
            if ((uint)pos < (uint)span.Length)
            {
                span[pos] = value;
                _position = pos + 1;
            }
            else
            {
                GrowPush(value);
            }
        }

        public void PushMany(params ReadOnlySpan<T> values)
        {
            int pos = _position;
            if (pos > _span.Length - values.Length)
            {
                Grow(values.Length);
            }

            values.CopyTo(_span.Slice(_position));
            _position += values.Length;
        }
        
        public void RepeatPush(int count, T value)
        {
            if (_position > _span.Length - count)
            {
                Grow(count);
            }

            Span<T> dest = _span.Slice(_position, count);
            dest.Fill(value);
            _position += count;
        }

     
      
        public void InsertRepeat(int index, int count, T value)
        {
            if (_position > _span.Length - count)
            {
                Grow(count);
            }

            int remaining = _position - index;
            _span.Slice(index, remaining).CopyTo(_span.Slice(index + count));
            _span.Slice(index, count).Fill(value);
            _position += count;
        }
        

        /// <summary>
        /// Get a pinnable reference to the first <typeparamref name="T"/> item in the stack.
        /// </summary>
        /// <remarks>
        /// This overload is pattern matched in the C# 7.3+ compiler so you can omit the explicit method call,
        /// and write eg <c>fixed (T* ptr = builder)</c>
        /// </remarks>
        public ref T GetPinnableReference()
        {
            return ref MemoryMarshal.GetReference(_span);
        }

       
        public ReadOnlySpan<T> AsSpan() => _span.Slice(0, _position);
        public ReadOnlySpan<T> AsSpan(int start) => _span.Slice(start, _position - start);
        public ReadOnlySpan<T> AsSpan(Index start) => _span.Slice(0, _position)[start..];
        public ReadOnlySpan<T> AsSpan(int start, int length) => _span.Slice(start, length);
        public ReadOnlySpan<T> AsSpan(Range range) => _span.Slice(0, _position)[range];

      

        public override string ToString()
        {
            string s = _span.Slice(0, _position).ToString();
            Dispose();
            return s;
        }

      

      

     

   
      

      
    
      

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            int origPos = _position;
            if (origPos > _span.Length - length)
            {
                Grow(length);
            }

            _position = origPos + length;
            return _span.Slice(origPos, length);
        }

     

        /// <summary>
        /// Resize the internal buffer either by doubling current buffer size or
        /// by adding <paramref name="additionalCapacityBeyondPos"/> to
        /// <see cref="_position"/> whichever is greater.
        /// </summary>
        /// <param name="additionalCapacityBeyondPos">
        /// Number of chars requested beyond current position.
        /// </param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            Debug.Assert(additionalCapacityBeyondPos > 0);
            Debug.Assert(_position > _span.Length - additionalCapacityBeyondPos, "Grow called incorrectly, no resize is needed.");

            const uint ArrayMaxLength = 0x7FFFFFC7; // same as Array.MaxLength

            // Increase to at least the required size (_pos + additionalCapacityBeyondPos), but try
            // to double the size if possible, bounding the doubling to not go beyond the max array length.
            int newCapacity = (int)Math.Max(
                (uint)(_position + additionalCapacityBeyondPos),
                Math.Min((uint)_span.Length * 2, ArrayMaxLength));

            // Make sure to let Rent throw an exception if the caller has a bug and the desired capacity is negative.
            // This could also go negative if the actual required length wraps around.
            char[] poolArray = ArrayPool<char>.Shared.Rent(newCapacity);

            _span.Slice(0, _position).CopyTo(poolArray);

            char[]? toReturn = _rentedArray;
            _span = _rentedArray = poolArray;
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            char[]? toReturn = _rentedArray;
            this = default; // for safety, to avoid using pooled array if this instance is erroneously appended to again
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }
    }

    }