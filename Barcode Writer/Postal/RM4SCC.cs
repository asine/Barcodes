﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Barcode_Writer
{
    public class RM4SCC : BarcodeBase
    {
        private const int START = '(';
        private const int STOP = ')';

        public const string DEFAULTDPS1 = "9U";
        public const string DEFAULTDPS2 = "9W";
        public const string DEFAULTDPS3 = "9X";
        public const string DEFAULTDPS4 = "9Y";
        public const string DEFAULTDPS5 = "9Z";

        protected override void Init()
        {
            //DPS [1-9][ABD-HJLNP-UW-Z]
            AllowedCharsPattern = new System.Text.RegularExpressions.Regex("^[a-z0-9]{5-7}([1-9][ABD-HJLNP-UW-Z])?$");

            AddChecksum += new EventHandler<AddChecksumEventArgs>(RM4SCC_AddChecksum);

            DefaultSettings.IsTextShown = false;
            DefaultSettings.BarHeight = 12;
            DefaultSettings.MediumHeight = 8;
            DefaultSettings.ShortHeight = 4;
        }

        void RM4SCC_AddChecksum(object sender, AddChecksumEventArgs e)
        {
            int[] values = new int[PatternSet.Count];
            PatternSet.Keys.CopyTo(values, 0);

            int rowTotal = 0, colTotal = 0, tmp;

            for(int i = 1;i < e.Codes.Count -1; i++)
            {
                int index = Array.IndexOf(values, e.Codes[i]);
                tmp = (index / 6) + 1;
                rowTotal += (tmp == 6 ? 0 : tmp);

                tmp = (index % 6) + 1;
                colTotal += (tmp == 6 ? 0 : tmp);
            }

            rowTotal = rowTotal % 6;
            rowTotal = rowTotal == 0 ? 6 : rowTotal - 1;

            colTotal = colTotal % 6;
            colTotal = colTotal == 0 ? 5 : colTotal - 1;

            e.Codes.Insert(e.Codes.Count - 1, values[(rowTotal * 6) + colTotal]);
        }

        protected override void CreatePatternSet()
        {
            PatternSet = new Dictionary<int, Pattern>();

            PatternSet.Add('0', Pattern.Parse("ttff"));
            PatternSet.Add('1', Pattern.Parse("tdaf"));
            PatternSet.Add('2', Pattern.Parse("tdfa"));
            PatternSet.Add('3', Pattern.Parse("dtaf"));
            PatternSet.Add('4', Pattern.Parse("dtfa"));
            PatternSet.Add('5', Pattern.Parse("ddaa"));
            PatternSet.Add('6', Pattern.Parse("tadf"));
            PatternSet.Add('7', Pattern.Parse("tftf"));
            PatternSet.Add('8', Pattern.Parse("tfda"));
            PatternSet.Add('9', Pattern.Parse("datf"));

            PatternSet.Add('A', Pattern.Parse("dada"));
            PatternSet.Add('B', Pattern.Parse("dfta"));
            PatternSet.Add('C', Pattern.Parse("tafd"));
            PatternSet.Add('D', Pattern.Parse("tfad"));
            PatternSet.Add('E', Pattern.Parse("tfft"));
            PatternSet.Add('F', Pattern.Parse("daad"));
            PatternSet.Add('G', Pattern.Parse("daft"));
            PatternSet.Add('H', Pattern.Parse("dfat"));
            PatternSet.Add('I', Pattern.Parse("atdf"));
            PatternSet.Add('J', Pattern.Parse("adtf"));
            PatternSet.Add('K', Pattern.Parse("adda"));
            PatternSet.Add('L', Pattern.Parse("fttf"));
            PatternSet.Add('M', Pattern.Parse("ftda"));
            PatternSet.Add('N', Pattern.Parse("fdta"));
            PatternSet.Add('O', Pattern.Parse("atfd"));
            PatternSet.Add('P', Pattern.Parse("adad"));
            PatternSet.Add('Q', Pattern.Parse("adft"));
            PatternSet.Add('R', Pattern.Parse("ftad"));
            PatternSet.Add('S', Pattern.Parse("ftft"));
            PatternSet.Add('T', Pattern.Parse("fdat"));
            PatternSet.Add('U', Pattern.Parse("aadd"));
            PatternSet.Add('V', Pattern.Parse("aftd"));
            PatternSet.Add('W', Pattern.Parse("afdt"));
            PatternSet.Add('X', Pattern.Parse("fatd"));
            PatternSet.Add('Y', Pattern.Parse("fadt"));
            PatternSet.Add('Z', Pattern.Parse("fftt"));

            PatternSet.Add(START, Pattern.Parse("a"));
            PatternSet.Add(STOP, Pattern.Parse("f"));

        }

        protected override string ParseText(string value, CodedValueCollection codes)
        {
            value = base.ParseText(value, codes);

            foreach (var item in value.ToCharArray())
            {
                codes.Add(item);
            }

            codes.Insert(0, START);
            codes.Add(STOP);

            return value;
        }
    }
}
