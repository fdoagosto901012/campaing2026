using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class UserLogPagination
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public List<userlogDTO> Data { get; set; }

        public UserLogPagination(int totalItems, List<userlogDTO> data, int page, int pageSize = 10)
        {
            // Total de paginación a mostrar
            int _totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            // Pagina actual
            int _currentPage = page != null ? (int)page : 1;
            // Paginación inicia con
            int _startPage = _currentPage - 5;
            // Paginación termina con
            int _endPage = _currentPage + 4;
            if (_startPage <= 0)
            {
                _endPage -= (_startPage - 1);
                _startPage = 1;
            }
            if (_endPage > _totalPages)
            {
                _endPage = _totalPages;
                if (_endPage > 10)
                {
                    _startPage = _endPage - 9;
                }
            }
            // Propiedades de paginación
            TotalItems = totalItems;
            CurrentPage = _currentPage;
            PageSize = pageSize;
            TotalPages = _totalPages;
            StartPage = _startPage;
            EndPage = _endPage;
            Data = data;
        }
    }
}