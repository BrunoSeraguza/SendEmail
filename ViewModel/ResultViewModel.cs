using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogapi.ViewModel
{
    public class ResultViewModel<T>
    {
        public T Data { get; private set; }
        public List<string> Error { get; private set; } = new();

        public ResultViewModel(T data, List<string> error)
        {
            Data = data;
            Error = error;
            
        }
        public ResultViewModel(T data)
        {
            Data = data;
            
        }
        public ResultViewModel(List<string> error)
        {
            Error=error;
        }

        public ResultViewModel(string erro)
        {
            Error.Add(erro);
            
        }
        
    }
}