using CypherKeeper.Model;
using CypherKeeper.AuthLayer.Utility;
using CypherKeeper.DataAccess.GoogleSheets.Interface;
using GoogleSheetsCrudLibrary;

namespace CypherKeeper.DataAccess.GoogleSheets.Impl
{
    public class TbIconsDataAccess : ITbIconsDataAccess
    {
        private CommonFunctions _CF { get; set; }
        private string SheetId { get; set; }
        private GoogleSheetsCrud _GoogleSheetsCrud { get; set; }
        public TbIconsDataAccess(string sheetId, CommonFunctions _cf)
        {
            try
            {
                SheetId = sheetId;
                _CF = _cf;
                _GoogleSheetsCrud = new GoogleSheetsCrud(SheetId, "TEST", _cf.GetGoogleSheetAPIJSON());
            }
            catch (Exception) { }
        }

        public List<tbIconsModel> Get(int page = 1, int itemsPerPage = 100, bool onlyNonDeleted = true)
        {
            var FinalData = _GoogleSheetsCrud.GetList<tbIconsModel>(-1, -1);
            if (onlyNonDeleted)
            {
                FinalData = FinalData.Where(x => !x.isDeleted).ToList();
            }
            if(page < 0 || itemsPerPage < 0)
            {
                return FinalData;
            }

            FinalData = _CF.GetByPage(FinalData, page, itemsPerPage);

            return FinalData;
        }

        public tbIconsModel GetById(Guid Id, bool onlyNonDeleted = true)
        {
            var FinalData = _GoogleSheetsCrud.GetList<tbIconsModel>(-1, -1);
            if (onlyNonDeleted)
            {
                FinalData = FinalData.Where(x => !x.isDeleted).ToList();
            }
            return FinalData.Where(x => x.Id == Id).FirstOrDefault();
        }

        public tbIconsModel Add(tbIconsModel model)
        {
            model.Id = Guid.NewGuid();
            var result = _GoogleSheetsCrud.Add(model);
            return model;
        }

        public bool Update(Guid Id, tbIconsModel model)
        {
            model.Id = Id;
            var rowNumber = _GoogleSheetsCrud.GetRowNumbers<tbIconsModel>("Id", Id.ToString());
            var result = _GoogleSheetsCrud.Update(model, rowNumber[0]);
            return true;
        }

        public bool Delete(Guid Id)
        {
            var FinalData = _GoogleSheetsCrud.GetList<tbIconsModel>(-1, -1);
            var currentRow = FinalData.Where(x => x.Id == Id).FirstOrDefault();
            if(currentRow == null)
            {
                return false;
            }
            currentRow.isDeleted = true;
            currentRow.DeletedDate = DateTime.UtcNow;

            var rowNumber = _GoogleSheetsCrud.GetRowNumbers<tbIconsModel>("Id", Id.ToString());
            var result = _GoogleSheetsCrud.Update(currentRow, rowNumber[0]);

            return true;
        }

        public bool Restore(Guid Id)
        {
            var FinalData = _GoogleSheetsCrud.GetList<tbIconsModel>(-1, -1);
            var currentRow = FinalData.Where(x => x.Id == Id).FirstOrDefault();
            if (currentRow == null)
            {
                return false;
            }
            currentRow.isDeleted = false;

            var rowNumber = _GoogleSheetsCrud.GetRowNumbers<tbIconsModel>("Id", Id.ToString());
            var result = _GoogleSheetsCrud.Update(currentRow, rowNumber[0]);

            return true;
        }

        public int Total(bool onlyNonDeleted = true)
        {
            var FinalData = _GoogleSheetsCrud.GetList<tbIconsModel>(-1, -1);
            if (onlyNonDeleted)
            {
                FinalData = FinalData.Where(x => !x.isDeleted).ToList();
            }
            return FinalData.Count();
        }
    }
}
