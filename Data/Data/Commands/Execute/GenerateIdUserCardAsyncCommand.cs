using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Data.Commands.Execute
{
    public class GenerateIdUserCardAsyncCommand : IRequest<long>
    {
        public long UserId { get; set; }

        public class GenerateIdUserCardAsyncCommandHandle : IRequestHandler<GenerateIdUserCardAsyncCommand, long>
        {
            public async Task<long> Handle(GenerateIdUserCardAsyncCommand request, CancellationToken cancellationToken)
            {
                return await Task.Run(() =>
                {
                    var time = DateTime.Now.ToUniversalTime();
                    return long.Parse($"{request.UserId}{time.Year}{time.Month}{time.Day}{time.Hour}{time.Minute}{time.Second}");
                });
            }
        }
    }
}
