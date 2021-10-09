using DotNetCore01.Data;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore01.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly IBookmark _bookmark;

        public WorkerService(IBookmark bookmark)
        {
            _bookmark = bookmark;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*
             * 부모클래스가 추상클래스가 아닐 때는
             * base.ExecuteAsync(stoppingToken);
             * 추상클래스는 이렇게 사용X
             * 스스로 무슨 일을 할 수가 없다.
             * 단지 자신을 상속한 자식클래스에게 틀을 제공해 주고
             * 자식 클래스가 내용을 채워넣어 사용하도록 합니다.
             * 여기서도
             */
            while (!stoppingToken.IsCancellationRequested)
            {
                string name = "asp.net";
                var bookmark = new Bookmark
                {
                    Name = name,
                    Url = "http://asp.net",
                    Description = "asp.net site",
                    ModifiedDate = DateTime.UtcNow
                };

                int createCount = await _bookmark
                                        .CreateBookmarkAsync(bookmark);
                if (createCount < 1)
                {
                    Console.Write($"'{name}' 북마크 정보를" +
                                  " 등록하지 못했습니다.");
                }
                else
                {
                    Console.Write($"'{name}' 북마크 정보를" +
                                  " 성공적으로 등록했습니다.");

                    #region Transaction
                    int transCount = await _bookmark
                              .ThrowAwayDataInTheBookmarkTrashCanAsync(name);

                    if (transCount < 2)
                    {
                        Console.Write($"'{name}' 북마크 정보를" +
                                      " 삭제하지 못했습니다.");
                    }
                    else
                    {
                        Console.Write($"'{name}' 북마크 정보를" +
                                      " 성공적으로 삭제했습니다.");
                    }
                    #endregion
                }

                /*
                 * 1 millisecond => 1/1,000초
                 * 비동기에서는 1초를 지연시킨 후에 완료되는
                 * 취소 가능한 작업으로 만드는 Delay method
                 * 같은 것들을 지정해 주는 것이 좋습니다.
                 */
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
