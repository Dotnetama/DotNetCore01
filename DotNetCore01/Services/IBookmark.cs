using DotNetCore01.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore01.Services
{
    public interface IBookmark
    {
        /// <summary>
        /// 북마크 정보를 추가하기 위한 메서드
        /// </summary>
        /// <param name="model">북마크 도메인 모델</param>
        /// <returns></returns>
        Task<int> CreateBookmarkAsync(Bookmark model);
        /// <summary>
        /// [Transaction] 북마크 정보를 휴지통에 보관하고,
        /// 북마크 정보를 삭제하는 메서드
        /// </summary>
        /// <param name="name">북마크 이름</param>
        /// <returns></returns>
        Task<int> ThrowAwayDataInTheBookmarkTrashCanAsync(string name);
    }
}
