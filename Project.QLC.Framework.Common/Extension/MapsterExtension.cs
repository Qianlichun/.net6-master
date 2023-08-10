using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.QLC.Framework.Common.Extension
{
    /// <summary>
    /// 方法名称：MapsterExtension
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/3 16:49:54
    /// </summary>
    public static class MapsterExtension
    {
        #region 实体映射
        /// <summary>
        /// 1.1、类型映射_默认字段一一对应
        /// T需要映射后的实体 = 需要映射的实体.Adapt<T需要映射后的实体>();
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="tSource">源数据</param>
        /// <returns></returns>
        public static TDestination MapsterTo<TSource, TDestination>(this TSource tSource) where TSource : class where TDestination : class
        {
            if (tSource == null) return default;
            return tSource.Adapt<TDestination>();
        }

        /// <summary>
        /// 1.2、类型映射_默认字段一一对应 (映射到现有对象)
        /// T需要映射后的实体 = 需要映射的实体.Adapt<T需要映射后的实体>();
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="tDestination">目标对象</param>
        /// <param name="tSource">源数据</param>
        /// <returns></returns>
        public static TDestination MapsterMedgeTo<TSource, TDestination>(this TSource tSource, TDestination tDestination) where TSource : class where TDestination : class
        {
            if (tSource == null) return default;
            var typeAdapterConfig = new TypeAdapterConfig();
            typeAdapterConfig.ForType<TSource, TDestination>()
                .IgnoreNullValues(true);
            var mapper = new Mapper(typeAdapterConfig);
            return mapper.Map(tSource, tDestination);
        }

        #endregion 实体映射

        #region 列表映射
        /// <summary>
        /// 3、集合列表类型映射,默认字段名字一一对应
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源数据</param>
        /// <returns></returns>
        public static IEnumerable<TDestination> MapsterToList<TSource, TDestination>(this IEnumerable<TSource> sources)
            where TSource : class
            where TDestination : class
        {
            if (sources == null) return new List<TDestination>();

            return sources.Adapt<List<TDestination>>();
        }

        #endregion 列表映射


        /// <summary>
        /// 可以处理复杂映射
        /// </summary>
        /// <typeparam name="TIn">输入类</typeparam>
        /// <typeparam name="TOut">输出类</typeparam>
        /// <param name="expression">表达式目录树,可以为null</param>
        /// <param name="tIn">输入实例</param>
        /// <returns></returns>
        public static TOut MapsterOfProperties<TIn, TOut>(this TIn tIn, Expression<Func<TIn, TOut>> expression = null)
            where TIn : class
            where TOut : class
        {
            ParameterExpression parameterExpression = null;
            List<MemberBinding> memberBindingList = new List<MemberBinding>();
            parameterExpression = Expression.Parameter(typeof(TIn), "p");

            if (expression != null)
            {
                parameterExpression = expression.Parameters[0];
                if (expression.Body != null)
                {
                    memberBindingList.AddRange((expression.Body as MemberInitExpression).Bindings);
                }
            }
            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());

            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[]
            {
                    parameterExpression
            });
            Func<TIn, TOut> func = lambda.Compile();//获取委托
            return func.Invoke(tIn);
        }

        public static IEnumerable<TOut> MapsterOfPropertiesList<TIn, TOut>(this IEnumerable<TIn> tIn, Expression<Func<TIn, TOut>> expression = null)
            where TIn : class
            where TOut : class
        {
            List<TOut> list = new List<TOut>();
            foreach (var de in tIn)
            {
                list.Add(de.MapsterOfProperties<TIn, TOut>(expression));
            }
            return list;
        }
    }
}
