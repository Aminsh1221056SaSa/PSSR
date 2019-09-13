using PSSR.Common;
using PSSR.DataLayer.EfCode;
using PSSR.ServiceLayer.FormDictionaryServices.QueryObjects;
using PSSR.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSSR.ServiceLayer.FormDictionaryServices.Concrete
{
    public class FormDictionaryFilterDropdownService
    {
        private readonly EfCoreContext _db;

        public FormDictionaryFilterDropdownService(EfCoreContext db)
        {
            _db = db;
        }

        public IEnumerable<DropdownTuple> GetFilterDropDownValues(FormDictionaryFilterBy filterBy)
        {
            switch (filterBy)
            {
                case FormDictionaryFilterBy.NoFilter:
                    return new List<DropdownTuple>();
                case FormDictionaryFilterBy.ByType:
                    return Enum.GetValues(typeof(FormDictionaryType))
                        .Cast<FormDictionaryType>().Select(v => new DropdownTuple
                        {
                            Value=v.ToString(),
                            Text=v.ToString()
                        });
                    
                case FormDictionaryFilterBy.StationType:
                    return _db.ProjectRoadMaps.Select(v => new DropdownTuple
                        {
                            Value = v.Id.ToString(),
                            Text = v.Name.ToString()
                        });

                case FormDictionaryFilterBy.Descipline:
                    return _db.Desciplines.Select(v => new DropdownTuple
                    {
                        Value = v.Id.ToString(),
                        Text = v.Name.ToString()
                    });
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}
