using DoctorConsult.Models;
using DoctorConsult.ViewModels.SectionVM;

namespace DoctorConsult.Domain.Interfaces
{
    public interface ISectionRepository
    {
        List<IndexSectionVM.GetData> GetAll();

        List<IndexSectionVM.GetData> SelectSectionsInAbout();
        EditSectionVM GetById(int id);
        int Add(CreateSectionVM SectionObj);
        int Update(EditSectionVM SectionObj);
        int Delete(int id);
        public int UpdateSectionImageAfterInsert(CreateSectionVM modelObj);
    }
}
