namespace Claim_Skimmer
{

    public enum HorizontalAlign
    {

        Centered,
        Fill,
        General,
        Left,
        Right
    }

    internal sealed class BIFF
    {

        public const ushort BOFRecord = 0x0209;
        public const ushort CodepageRecord = 0x0042;
        public const ushort ColumnInfoRecord = 0x007D;
        public const ushort DefaultColor = 0x7fff;
        public const ushort EOFRecord = 0x0A;
        public const ushort ExtendedRecord = 0x0243;
        public const ushort FontRecord = 0x0231;
        public const ushort FooterRecord = 0x0015;
        public const ushort FormatRecord = 0x001E;
        public const ushort HeaderRecord = 0x0014;
        public const ushort LabelRecord = 0x0204;
        public const ushort NumberRecord = 0x0203;
        public const ushort StyleRecord = 0x0293;
        public const ushort WindowProtectRecord = 0x0019;
        public const ushort XFRecord = 0x0243;
    }

    public class ExcelSpreadsheet
    {

        public class SupportedColors
        {

            public static ExcelSpreadsheetColor Black
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Black, 0); }
            }

            public static ExcelSpreadsheetColor White
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.White, 1); }
            }

            public static ExcelSpreadsheetColor Red
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Red, 2); }
            }

            public static ExcelSpreadsheetColor Green
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Green, 3); }
            }

            public static ExcelSpreadsheetColor Blue
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Blue, 4); }
            }

            public static ExcelSpreadsheetColor Yellow
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Yellow, 5); }
            }

            public static ExcelSpreadsheetColor Magenta
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Magenta, 6); }
            }

            public static ExcelSpreadsheetColor Cyan
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Cyan, 7); }
            }

            public static ExcelSpreadsheetColor DarkRed
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.DarkRed, 0x10); }
            }

            public static ExcelSpreadsheetColor DarkGreen
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.DarkGreen, 0x11); }
            }

            public static ExcelSpreadsheetColor DarkBlue
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.DarkBlue, 0x12); }
            }

            public static ExcelSpreadsheetColor Olive
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Olive, 0x13); }
            }

            public static ExcelSpreadsheetColor Purple
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Purple, 0x14); }
            }

            public static ExcelSpreadsheetColor Teal
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Teal, 0x15); }
            }

            public static ExcelSpreadsheetColor Silver
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Silver, 0x16); }
            }

            public static ExcelSpreadsheetColor Gray
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.Gray, 0x17); }
            }

            public static ExcelSpreadsheetColor WindowText
            {

                get { return new ExcelSpreadsheetColor(System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.WindowText), 0x18); }
            }

            public static ExcelSpreadsheetColor WindowBackground
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Window), 0x19); }
            }

            public static ExcelSpreadsheetColor Automatic
            {
                get { return new ExcelSpreadsheetColor(System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.WindowText), BIFF.DefaultColor); }
            }
        }

        public string Author { get; set; }

        private ushort[] clBegin = { BIFF.BOFRecord, 0x8, 0x0, 0x10, 0x0, 0x0 };
        private ushort[] clEnd = { BIFF.EOFRecord, 00 };

        private System.Collections.Generic.Dictionary<long, ExcelSpreadsheetCellFormat> cells = new System.Collections.Generic.Dictionary<long, ExcelSpreadsheetCellFormat>();
        private System.Collections.Generic.List<ExcelSpreadsheetColumn> columns = new System.Collections.Generic.List<ExcelSpreadsheetColumn>();
        private System.Collections.Generic.List<ExcelSpreadsheetFont> fonts = new System.Collections.Generic.List<ExcelSpreadsheetFont>()
        {

            new ExcelSpreadsheetFont(SupportedColors.Automatic, "Arial", 10)
        };
        private System.Collections.Generic.List<string> formats = new System.Collections.Generic.List<string>()
        {

            "General"
        };
        private System.Collections.Generic.List<ExcelSpreadsheetCellFontFormat> fx = new System.Collections.Generic.List<ExcelSpreadsheetCellFontFormat>();
        private System.Collections.Generic.List<int> rows = new System.Collections.Generic.List<int>();

        internal ExcelSpreadsheetCellFormat GetCellInfo(int row, int column)
        {

            long key = HashCode(row, column);
            ExcelSpreadsheetCellFormat result;
            if (cells.TryGetValue(key, out result))
                return result;
            else
            {
                if (rows.IndexOf(row) == -1)
                    rows.Add(row);
                result = new ExcelSpreadsheetCellFormat(this);
                result.RowIndex = row;
                result.ColumnIndex = column;
                cells.Add(key, result);
                return result;
            }
        }

        internal System.Collections.Generic.List<string> Formats
        {
            get { return formats; }
        }

        internal System.Collections.Generic.List<ExcelSpreadsheetFont> Fonts
        {
            get { return fonts; }
        }

        internal System.Collections.Generic.List<ExcelSpreadsheetCellFontFormat> FX
        {
            get { return fx; }
        }

        private static long HashCode(int row, int column)
        {
            return (((long)row << 32) + (long)column);
        }

        public ExcelSpreadsheetCell Cell(int row, int column) { return new ExcelSpreadsheetCell(row, column, this); }

        public ExcelSpreadsheetCell this[int row, int column] { get { return Cell(row, column); } }

        public void ColumnWidth(int column, int width)
        {

            int idx;
            ExcelSpreadsheetColumn info = new ExcelSpreadsheetColumn();
            info.Index = column;
            info.Width = width;
            idx = columns.IndexOf(info);
            if (idx == -1)
            {
                columns.Add(info);
                idx = columns.IndexOf(info);
            }
            else
                columns[idx].Width = width;
        }

        public void Save(System.IO.Stream stream)
        {

            BuildInternalTables();

            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);
            WriteUshortArray(writer, clBegin);

            WriteAuthorRecord(writer);
            WriteCodepageRecord(writer);
            WriteFontTable(writer);
            WriteHeaderRecord(writer);
            WriteFooterRecord(writer);
            WriteFormatTable(writer);
            WriteWindowProtectRecord(writer);
            WriteFXTable(writer);
            WriteStyleTable(writer);

            for (int i = 0; i < columns.Count; i++)
            {
                WriteColumnInfoRecord(writer, columns[i]);
            }

            rows.Sort();

            for (int i = 0; i < rows.Count; i++)
            {
                foreach (var cell in cells)
                {
                    if (cell.Value.RowIndex == rows[i])
                        WriteCellValue(writer, cell.Value);
                }
            }


            WriteUshortArray(writer, clEnd);
            writer.Flush();
        }

        public ExcelSpreadsheetCell WriteCell(int row, int column, object value)
        {

            ExcelSpreadsheetCell cell = Cell(row, column);

            cell.Value = value;

            return cell;
        }

        private void WriteCellDate(System.IO.BinaryWriter writer, ExcelSpreadsheetCellFormat cell)
        {
            System.DateTime value;
            if (cell.Value is System.DateTime)
            {
                value = (System.DateTime)cell.Value;
                System.DateTime baseDate = new System.DateTime(1899, 12, 31);
                System.TimeSpan ts = value - baseDate;

                double days = (double)(ts.Days + 1);
                if (days >= 60)
                {
                    days += 1;
                }

                ushort[] clData = { BIFF.NumberRecord, 14, (ushort)cell.RowIndex, (ushort)cell.ColumnIndex, (ushort)cell.FXIndex };
                WriteUshortArray(writer, clData);
                writer.Write(days);
            }
        }

        private void WriteCellEmpty(System.IO.BinaryWriter writer, ExcelSpreadsheetCellFormat cell)
        {

            ushort[] clData = { 0x0201, 6, 0, 0, 15 };
            clData[2] = (ushort)cell.RowIndex;
            clData[3] = (ushort)cell.ColumnIndex;

            WriteUshortArray(writer, clData);
        }

        private void WriteCellNumber(System.IO.BinaryWriter writer, ExcelSpreadsheetCellFormat cell)
        {
            double dValue = System.Convert.ToDouble(cell.Value);
            ushort[] clData = { BIFF.NumberRecord, 14, (ushort)cell.RowIndex, (ushort)cell.ColumnIndex, (ushort)cell.FXIndex };
            WriteUshortArray(writer, clData);
            writer.Write(dValue);
        }

        private void WriteCellString(System.IO.BinaryWriter writer, ExcelSpreadsheetCellFormat cell)
        {

            string value;
            if (cell.Value is string)
                value = (string)cell.Value;
            else
                value = cell.Value.ToString();
            if (value.Length > 255)
                value = value.Substring(0, 255);
            ushort[] clData = { BIFF.LabelRecord, 0, 0, 0, 0, 0 };
            byte[] plainText = System.Text.Encoding.UTF8.GetBytes(value);
            int iLen = plainText.Length;
            clData[1] = (ushort)(8 + iLen);
            clData[2] = (ushort)cell.RowIndex;
            clData[3] = (ushort)cell.ColumnIndex;
            clData[4] = (ushort)cell.FXIndex;
            clData[5] = (ushort)iLen;
            WriteUshortArray(writer, clData);
            writer.Write(plainText);
        }

        private void WriteCellValue(System.IO.BinaryWriter writer, ExcelSpreadsheetCellFormat cell)
        {

            if (cell.Value == null)
                WriteCellEmpty(writer, cell);
            else
                if (cell.Value is string)
                WriteCellString(writer, cell);
            else
                    if (IsNumber(cell.Value))
                WriteCellNumber(writer, cell);
            else
                        if (cell.Value is System.DateTime)
                WriteCellDate(writer, cell);
            else
                WriteCellString(writer, cell);
        }

        private void BuildInternalTables()
        {

            ExcelSpreadsheetCellFontFormat info;

            foreach (var cell in cells)
            {
                info = new ExcelSpreadsheetCellFontFormat(cell.Value);
                if (cell.Value.Document.FX.IndexOf(info) == -1)
                    cell.Value.Document.FX.Add(info);

                cell.Value.FXIndex = (byte)(cell.Value.Document.FX.IndexOf(info) + 21);
            }
        }

        private static bool IsNumber(object value)
        {

            if (value is sbyte) return true;
            if (value is byte) return true;
            if (value is short) return true;
            if (value is ushort) return true;
            if (value is int) return true;
            if (value is uint) return true;
            if (value is long) return true;
            if (value is ulong) return true;
            if (value is float) return true;
            if (value is double) return true;
            if (value is decimal) return true;
            return false;
        }

        private static void WriteByteArray(System.IO.BinaryWriter writer, byte[] value)
        {
            for (int i = 0; i < value.Length; i++)
                writer.Write(value[i]);
        }

        private void WriteAuthorRecord(System.IO.BinaryWriter writer)
        {
            ushort[] clData = { 0x005c, 32 };
            string writerName;
            if (string.IsNullOrEmpty(Author))
                writerName = string.Empty.PadRight(31);
            else
            {
                writerName = Author.Substring(0, Author.Length > 31 ? 31 : Author.Length);
                writerName = writerName.PadRight(31);
            }

            WriteUshortArray(writer, clData);
            writer.Write(writerName);
        }

        private void WriteCodepageRecord(System.IO.BinaryWriter writer)
        {
            ushort[] clData = { BIFF.CodepageRecord, 0x2, 0 };
            clData[2] = (ushort)System.Globalization.CultureInfo.CurrentCulture.TextInfo.ANSICodePage;
            WriteUshortArray(writer, clData);
        }

        private void WriteColumnInfoRecord(System.IO.BinaryWriter writer, ExcelSpreadsheetColumn info)
        {
            ushort[] clData = { BIFF.ColumnInfoRecord, 12, (ushort)info.Index, (ushort)info.Index, (ushort)(info.Width * 256 / 7), 15, 0, 0 };
            WriteUshortArray(writer, clData);
        }

        private void WriteFooterRecord(System.IO.BinaryWriter writer)
        {

            ushort[] clData = { BIFF.FooterRecord, 0 };

            WriteUshortArray(writer, clData);
        }

        private void WriteFormat(System.IO.BinaryWriter writer, string value)
        {

            ushort[] clData = { BIFF.FormatRecord, 0 };
            byte[] plainText = System.Text.Encoding.ASCII.GetBytes(value);
            int iLen = plainText.Length;
            clData[1] = (ushort)(1 + iLen);
            WriteUshortArray(writer, clData);
            writer.Write((byte)iLen);
            writer.Write(plainText);
        }

        private void WriteFormatTable(System.IO.BinaryWriter writer)
        {
            for (int i = 0; i < formats.Count; i++)
            {
                WriteFormat(writer, formats[i]);
            }
        }

        private void WriteHeaderRecord(System.IO.BinaryWriter writer)
        {
            ushort[] clData = { BIFF.HeaderRecord, 0 };
            WriteUshortArray(writer, clData);
        }

        private void WriteFontRecord(System.IO.BinaryWriter writer, ExcelSpreadsheetFont font)
        {
            ushort[] clData = { BIFF.FontRecord, 0, 0, 0, font.Color.Index };
            byte[] plainText = System.Text.Encoding.ASCII.GetBytes(font.FontFamily);
            int iLen = plainText.Length;
            clData[1] = (ushort)(7 + iLen);
            clData[2] = (ushort)(font.Size * 20);
            int Flags = 0;
            if (font.Bold)
                Flags |= 1;
            if (font.Italic)
                Flags |= 2;
            if (font.Underline)
                Flags |= 4;
            if (font.Strikeout)
                Flags |= 8;
            clData[3] = (ushort)Flags;

            WriteUshortArray(writer, clData);
            writer.Write((byte)iLen);
            writer.Write(plainText);
        }

        private void WriteFontTable(System.IO.BinaryWriter writer)
        {

            foreach (var font in fonts) WriteFontRecord(writer, font);
        }

        private void WriteFXRecord(System.IO.BinaryWriter writer, ExcelSpreadsheetCellFontFormat info)
        {

            ushort[] clData = new ushort[2];
            clData[0] = BIFF.ExtendedRecord;
            clData[1] = 0x00C;
            WriteUshortArray(writer, clData);

            byte[] clValue = new byte[4];
            clValue[0] = (byte)info.FontIndex;
            clValue[1] = (byte)info.FormatIndex;
            clValue[2] = (byte)0x01;

            byte attr = 0;
            if (info.FontIndex > 0)
                attr |= 0x02;
            if (info.HorizontalAlignment != HorizontalAlign.General)
                attr |= 0x04;
            if (info.BackColor.Index != SupportedColors.Automatic.Index)
                attr |= 0x10;
            attr = (byte)(attr << 2);
            clValue[3] = attr;
            WriteByteArray(writer, clValue);

            //(orig & ~mask) | (input & mask)

            ushort horizontalAlignment = (ushort)info.HorizontalAlignment;

            ushort backgroundArea = 1;
            if (info.BackColor.Index != SupportedColors.Automatic.Index)
            {
                backgroundArea = (ushort)((backgroundArea & ~(ushort)0x07C0) | (info.BackColor.Index & (ushort)0x07C0 >> 6) << 6);
                backgroundArea = (ushort)((backgroundArea & ~(ushort)0xF800) | (SupportedColors.WindowText.Index & (ushort)0xF800 >> 11) << 11);
            }
            else
                backgroundArea = 0xCE00;

            ushort[] rest = { horizontalAlignment, backgroundArea, 0x0000, 0x0000 };
            WriteUshortArray(writer, rest);
        }

        private void WriteFXTable(System.IO.BinaryWriter writer)
        {

            ushort[][] clData =
            {

                new ushort[] {0x243, 0x0c, 0, 0x3f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x01, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x01, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x02, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x02, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0xf7f5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0, 0x01, 0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x2101, 0xfbf5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x1f01, 0xfbf5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x2001, 0xfbf5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x1e01, 0xfbf5, 0xfff0, 0xce00, 0, 0},
                new ushort[] {0x243, 0x0c, 0x0901, 0xfbf5, 0xfff0, 0xce00, 0, 0}
            };

            for (int i = 0; i < 21; i++) WriteUshortArray(writer, clData[i]);

            foreach (var info in fx) WriteFXRecord(writer, info);
        }

        private void WriteStyleTable(System.IO.BinaryWriter writer)
        {

            byte[][] clData =
            {

                new byte[] {0x10,0x80,0x03,0xFF},
                new byte[] {0x11,0x00,0x09,0x43,0x6F,0x6D,0x6D,0x61,0x20,0x5B,0x30,0x5D},
                new byte[] {0x12,0x80,0x04,0xFF},
                new byte[] {0x13,0x00,0x0C,0x43,0x75,0x72,0x72,0x65,0x6E,0x63,0x79,0x20,0x5B,0x30,0x5D},
                new byte[] {0x00,0x80,0x00,0xFF},
                new byte[] {0x14,0x80,0x05,0xFF}
            };

            ushort[] clHeader = new ushort[2];
            clHeader[0] = BIFF.StyleRecord;

            for (int i = 0; i < 6; i++)
            {
                clHeader[1] = (ushort)clData[i].Length;
                WriteUshortArray(writer, clHeader);
                WriteByteArray(writer, clData[i]);
            }

        }

        private static void WriteUshortArray(System.IO.BinaryWriter writer, ushort[] value)
        {

            for (int i = 0; i < value.Length; i++) writer.Write(value[i]);
        }

        private void WriteWindowProtectRecord(System.IO.BinaryWriter writer)
        {

            ushort[] clData = { BIFF.WindowProtectRecord, 2, 0 };

            WriteUshortArray(writer, clData);
        }

    }

    public class ExcelSpreadsheetCell
    {

        internal ExcelSpreadsheetCell(int row, int column, ExcelSpreadsheet spreadsheet)
        {

            format = spreadsheet.GetCellInfo(row, column);
            this.spreadsheet = spreadsheet;
        }

        public HorizontalAlign Alignment { get { return format.Alignment; } set { format.Alignment = value; } }
        public ExcelSpreadsheetColor BackColor { get { return format.BackColor; } set { format.BackColor = value; } }
        public ExcelSpreadsheetFont Font { get { return format.Font; } set { format.Font = value; } }
        public ExcelSpreadsheetColor ForeColor { get { return format.ForeColor; } set { format.ForeColor = value; } }
        public string Format
        {

            get { return format.Format; }

            set
            {
                format.Format = value;

                if (!spreadsheet.Formats.Contains(value)) spreadsheet.Formats.Add(value);
            }
        }
        public object Value { get { return format.Value; } set { format.Value = value; } }

        private ExcelSpreadsheetCellFormat format = null;
        private ExcelSpreadsheet spreadsheet = null;
    }

    internal class ExcelSpreadsheetCellFormat
    {

        public HorizontalAlign Alignment { get; set; }
        public ExcelSpreadsheetColor BackColor { get; set; }
        public int ColumnIndex { get; set; }
        public ExcelSpreadsheet Document { get; set; }
        public ExcelSpreadsheetFont Font { get; set; }
        public ExcelSpreadsheetColor ForeColor { get; set; }
        public string Format { get; set; }
        public byte FXIndex { get; set; }
        public int RowIndex { get; set; }
        public object Value { get { return value; } set { this.value = value; } }
        private object value = null;

        public ExcelSpreadsheetCellFormat(ExcelSpreadsheet document)
        {

            BackColor = ExcelSpreadsheet.SupportedColors.Automatic;
            Document = document;
            Font = document.Fonts[0];
            ForeColor = ExcelSpreadsheet.SupportedColors.Automatic;
        }
    }

    internal class ExcelSpreadsheetCellFontFormat
    {

        public ExcelSpreadsheetColor BackColor { get { return cellFormat.BackColor; } }
        public ExcelSpreadsheetFont Font { get { return cellFormat.Font; } }
        public int FontIndex { get { return fontIndex; } }
        public ExcelSpreadsheetColor ForeColor { get { return cellFormat.ForeColor; } }
        public string Format { get { return cellFormat.Format; } }
        public int FormatIndex { get { return formatIndex; } }
        public HorizontalAlign HorizontalAlignment { get { return cellFormat.Alignment; } }

        private ExcelSpreadsheetCellFormat cellFormat = null;
        private int fontIndex = 0;
        private int formatIndex = 0;

        public ExcelSpreadsheetCellFontFormat(ExcelSpreadsheetCellFormat cellFormat)
        {

            this.cellFormat = cellFormat;

            if (string.IsNullOrEmpty(cellFormat.Format)) formatIndex = 0;
            else formatIndex = cellFormat.Document.Formats.IndexOf(cellFormat.Format);

            ExcelSpreadsheetFont fontInfo = cellFormat.Font;

            fontIndex = cellFormat.Document.Fonts.IndexOf(fontInfo);

            if (fontIndex == -1)
            {

                cellFormat.Document.Fonts.Add(fontInfo);

                fontIndex = cellFormat.Document.Fonts.IndexOf(fontInfo);
            }

            if (fontIndex > 3) fontIndex++;
        }

        public override bool Equals(object obj)
        {

            if (obj is ExcelSpreadsheetCellFontFormat)
            {

                ExcelSpreadsheetCellFontFormat cellFontFormatToCheck = (ExcelSpreadsheetCellFontFormat)obj;

                return
                (

                    BackColor.Index == cellFontFormatToCheck.BackColor.Index &&
                    fontIndex == cellFontFormatToCheck.fontIndex &&
                    ForeColor.Index == cellFontFormatToCheck.ForeColor.Index &&
                    formatIndex == cellFontFormatToCheck.formatIndex &&
                    HorizontalAlignment == cellFontFormatToCheck.HorizontalAlignment
                );

            }

            return false;
        }
    }

    public class ExcelSpreadsheetColor
    {

        public System.Drawing.Color BaseColor { get { return baseColor; } }
        internal ushort Index { get; set; }

        private System.Drawing.Color baseColor;

        internal ExcelSpreadsheetColor(System.Drawing.Color baseColor, ushort index)
        {

            this.baseColor = baseColor;
            Index = index;
        }

        public override bool Equals(object obj)
        {

            if (obj is ExcelSpreadsheetColor) return (Index == ((ExcelSpreadsheetColor)obj).Index);

            return false;
        }

        public override int GetHashCode() { return Index; }
    }

    internal class ExcelSpreadsheetColumn
    {

        public int Index { get; set; }
        public int Width { get; set; }

        public override bool Equals(object obj)
        {

            if (obj is ExcelSpreadsheetColumn) return (Index == ((ExcelSpreadsheetColumn)obj).Index);

            return base.Equals(obj);
        }
    }

    public class ExcelSpreadsheetFont
    {

        public bool Bold { get; set; }
        public ExcelSpreadsheetColor Color { get; set; }
        public bool Italic { get; set; }
        public string FontFamily { get; set; }
        public int Size { get; set; }
        public bool Strikeout { get; set; }
        public bool Underline { get; set; }

        public ExcelSpreadsheetFont()
        {

            Color = ExcelSpreadsheet.SupportedColors.Black;
        }

        public ExcelSpreadsheetFont(ExcelSpreadsheetColor color, string fontFamily, int size)
        {

            Color = color;
            FontFamily = fontFamily;
            Size = size;
        }

        public ExcelSpreadsheetFont(string fontFamily, int size)
        {

            Color = ExcelSpreadsheet.SupportedColors.Black;
            FontFamily = fontFamily;
            Size = size;
        }

        public override bool Equals(object obj)
        {

            if (obj is ExcelSpreadsheetFont)
                return (((ExcelSpreadsheetFont)obj).GetHashCode() == GetHashCode());

            return false;
        }
    }
}
