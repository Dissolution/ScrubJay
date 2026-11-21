namespace ScrubJay.Debugging.Destinations.Console;

public static class ConsoleExtensions
{
    extension(System.Console)
    {
        public static void SetColors(ConsoleColors colors)
        {
            System.Console.ForegroundColor = colors.Foreground;
            System.Console.BackgroundColor = colors.Background;
        }

        public static ConsoleColorReset ChangeColors(ConsoleColorChange change)
        {
            ConsoleColor? foreReset;
            if (change.ChangesForeground(out var newFore))
            {
                foreReset = System.Console.ForegroundColor;
                System.Console.ForegroundColor = newFore;
            }
            else
            {
                foreReset = null;
            }

            ConsoleColor? backReset;
            if (change.ChangesBackground(out var newBack))
            {
                backReset = System.Console.BackgroundColor;
                System.Console.BackgroundColor = newBack;
            }
            else
            {
                backReset = null;
            }

            return new(new(foreReset, backReset));
        }
        
        public static void Write(ConsoleColorChange colors, string? str)
        {
            using (System.Console.ChangeColors(colors))
            {
                System.Console.Write(str);
            }
        }
    }

    extension(ConsoleColor color)
    {
        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => color switch
            {
                ConsoleColor.Black => nameof(ConsoleColor.Black),
                ConsoleColor.DarkBlue => nameof(ConsoleColor.DarkBlue),
                ConsoleColor.DarkGreen => nameof(ConsoleColor.DarkGreen),
                ConsoleColor.DarkCyan => nameof(ConsoleColor.DarkCyan),
                ConsoleColor.DarkRed => nameof(ConsoleColor.DarkRed),
                ConsoleColor.DarkMagenta => nameof(ConsoleColor.DarkMagenta),
                ConsoleColor.DarkYellow => nameof(ConsoleColor.DarkYellow),
                ConsoleColor.Gray => nameof(ConsoleColor.Gray),
                ConsoleColor.DarkGray => nameof(ConsoleColor.DarkGray),
                ConsoleColor.Blue => nameof(ConsoleColor.Blue),
                ConsoleColor.Green => nameof(ConsoleColor.Green),
                ConsoleColor.Cyan => nameof(ConsoleColor.Cyan),
                ConsoleColor.Red => nameof(ConsoleColor.Red),
                ConsoleColor.Magenta => nameof(ConsoleColor.Magenta),
                ConsoleColor.Yellow => nameof(ConsoleColor.Yellow),
                ConsoleColor.White => nameof(ConsoleColor.White),
                _ => "???",
            };
        }
    }
    
    extension(ConsoleColor? color)
    {
        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (color.HasValue)
                    return color.GetValueOrDefault().Name;
                return "___";
            }
        }
    }
}