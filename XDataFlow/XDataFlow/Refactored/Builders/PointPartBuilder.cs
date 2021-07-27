using System;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Refactored.Builders
{
    public class PointPartBuilder
    {
        private readonly Func<IMetaDataController> _metaDataControllerBuilder;
        private readonly Func<IGroupController> _groupControllerBuilder;

        public PointPartBuilder(
            Func<IMetaDataController> metaDataControllerBuilder,
            Func<IGroupController> groupControllerBuilder)
        {
            _metaDataControllerBuilder = metaDataControllerBuilder;
            _groupControllerBuilder = groupControllerBuilder;
        }
        public Func<TPointPart> GetTemplate<TPointPart>() where TPointPart : PointPart
        {
            return () =>
            {
               var pointPart = Activator.CreateInstance(typeof(TPointPart),
                   _metaDataControllerBuilder(),
                   _groupControllerBuilder());

               return (TPointPart) pointPart;
            };
        }
    }
}