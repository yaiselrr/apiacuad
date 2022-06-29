using System.Collections.Generic;

namespace API.Models.Dto
{
    public class ResponseAuthDto
    {
        public bool IsSuccess { get; set; } = true;

        public object Token { get; set; }

        public string Role { get; set; }

        public string DisplayMessage { get; set; }

        public bool Authenticated { get; set; } = false;

        public List<string> ErrorMessage { get; set; }
    }
}
