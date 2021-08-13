using System;

namespace PornJudger.Dto
{
    public class ImageDataDto
    {
        public Guid Id { get; set; }
        public Guid QrcodeId { get; set; }
        public Guid TakerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Porn { get; set; }

        public bool Judge { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
