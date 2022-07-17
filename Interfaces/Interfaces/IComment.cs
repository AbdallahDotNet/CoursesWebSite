using Interfaces.ViewModels.CommentVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Interfaces
{
    public interface IComment
    {
        Task<List<GetCommentViewModel>> GetAll();
        Task<SaveCommentViewModel> GetUpdatedComment(Guid id);
        Task<bool> Save(SaveCommentViewModel model);
        Task<bool> Delete(Guid id);
        Task<bool> Approving(Guid id);
    }
}
