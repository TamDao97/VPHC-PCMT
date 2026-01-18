using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document.Word
{
    public class ItemTextReplate
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// String or html
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Replate Type
        /// Defaut: 0 - Replate text
        /// </summary>
        public ReplateType Type { get; set; }

        /// <summary>
        /// Replate all
        /// Default: false
        /// </summary>
        public bool ReplateAll { get; set; }
    }

    public enum ReplateType
    {
        //
        // Summary:
        //     Repalte text
        Text = 0,
        //
        // Summary:
        //     Repalte Html
        Html = 1
    }
}
