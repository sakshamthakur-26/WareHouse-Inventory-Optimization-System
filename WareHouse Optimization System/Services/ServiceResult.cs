using System.Collections.Generic;
using WareHouse_Optimization_System.DTOs.Stock;
using WareHouse_Optimization_System.Models;
namespace WareHouse_Optimization_System.Services
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public static ServiceResult<T> Success(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                ErrorMessage = null
            };
        }
        public static ServiceResult<T> Failure(string errorMessage) => new ServiceResult<T> { 
            IsSuccess = false,
            ErrorMessage = errorMessage
        };

      
    }
}
