using PetaPoco;
using System;
using System.Reflection;

namespace Bootstrap.DataAccess
{
    internal class BootstrapDataAccessConventionMapper : IMapper
    {
        private readonly IMapper _mapper;

        public BootstrapDataAccessConventionMapper()
        {
            _mapper = new ConventionMapper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocoProperty"></param>
        /// <returns></returns>
        public ColumnInfo GetColumnInfo(PropertyInfo pocoProperty) => _mapper.GetColumnInfo(pocoProperty);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetProperty"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType) => _mapper.GetFromDbConverter(targetProperty, sourceType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocoType"></param>
        /// <returns></returns>
        public TableInfo GetTableInfo(Type pocoType)
        {
            var ti = _mapper.GetTableInfo(pocoType);
            ti.AutoIncrement = true;
            return ti;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceProperty"></param>
        /// <returns></returns>
        public Func<object, object> GetToDbConverter(PropertyInfo sourceProperty) => _mapper.GetToDbConverter(sourceProperty);
    }
}
