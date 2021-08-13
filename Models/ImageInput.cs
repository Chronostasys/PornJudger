using Microsoft.ML.Data;

namespace PornJudger.Models
{
    public class ImageInput
    {
        [ColumnName("Features"), LoadColumn(0)]
        public byte[] Image;

        [LoadColumn(1)]
        public string Label;
    }
}
