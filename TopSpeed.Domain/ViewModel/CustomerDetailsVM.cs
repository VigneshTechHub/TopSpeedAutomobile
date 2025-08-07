using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Models;

namespace TopSpeed.Domain.ViewModel
{
  public class CustomerDetailsVM
    {
        public PostModel Post{ get; set; }

        public List<PostModel> Posts { get; set; }
    }
}
