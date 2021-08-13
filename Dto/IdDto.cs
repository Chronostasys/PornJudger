using System;

namespace PornJudger.Dto
{
    public class IdDto
    {
        public IdDto()
        {

        }
        public IdDto(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
