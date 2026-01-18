using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Document.Word
{
    public class ItemImageReplate
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Replate all
        /// Default: false
        /// </summary>
        public bool ReplateAll { get; set; }

        /// <summary>
        /// Chiều rộng ảnh
        /// 
        /// </summary>
        public float? Width { get; set; }
        /// <summary>
        /// Chiều cao ảnh
        /// </summary>
        public float? Height { get; set; }
        /// <summary>
        /// Wrap text
        /// </summary>
        public TextWrappingStyle WrappingStyle { get; set; }

        public ItemImageReplate()
        {
            WrappingStyle = TextWrappingStyle.Inline;
        }    
    }
}
