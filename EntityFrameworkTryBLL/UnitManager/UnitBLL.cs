﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Zutu.Unit;
using DataContext;
using System.Data;

namespace EntityFrameworkTryBLL.UnitManager
{
    public class UnitBLL
    {
        /// <summary>
        /// 返回所有结果
        /// </summary>
        /// <returns></returns>
        public static List<UnitModel> getAllModels()
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    return context.UnitModels.ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到受影响属性的所有数据
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<UnitModel> getAllByCondition(string propertyName, int orderId)
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    List<UnitModel> rtUnitModels = new List<UnitModel>();
                    //首先得到受影响的属性的名称
                    List<string> influencedPtyNames = getInfluencedPties(propertyName);
                    //遍历受影响的属性
                    foreach (var ifn in influencedPtyNames)
                    {
                        //得到主动属性列表
                        var ptyNames = getPtyNames(ifn);
                        //得到影响当前属性取值的所有的条件的string组合
                        var conditionStrList = generateCondition(ptyNames, orderId);
                        var unitModels = context.UnitModels
                            .Where(s => s.Property == ifn);
                        foreach (var unitModel in unitModels)
                        {
                            if (string.IsNullOrEmpty(unitModel.Condition) || unitModel.Condition.Equals("ALL"))
                            {
                                rtUnitModels.Add(unitModel);
                                continue;
                            }
                            bool flag = false;
                            foreach (var cs in conditionStrList)
                            {
                                if (!unitModel.Condition.Contains(cs))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                            {
                                rtUnitModels.Add(unitModel);
                            }
                        }
                    }
                    return rtUnitModels;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据一列属性名得到属性的取值配对
        /// </summary>
        /// <param name="propertyNames"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private static List<string> generateCondition(List<string> propertyNames, int orderId)
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    List<string> ptyValues = new List<string>();
                    foreach (var propertyName in propertyNames)
                    {
                        var unitValue = context.UnitCurrentValues
                        .Where(s => s.OrderId == orderId
                        && s.PropertyName == propertyName)
                        .Select(s => s.Value)
                        .First();
                        string ptyValue = propertyName + ":" + unitValue;
                        //生成字符串类似与“name:value”
                        ptyValues.Add(ptyValue);
                    }
                    return ptyValues;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

        }


        /// <summary>
        /// 根据约束表，查看受一个属性影响的所有的被动属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static List<string> getInfluencedPties(string propertyName)
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    List<string> propertyNames = context.UnitConstraints
                        .Where(s => s.PropertyName == propertyName)
                        .Select(s => s.InfluencedPropertyName)
                        .ToList();
                    return propertyNames;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 根据被动属性，得到主动属性的列表
        /// </summary>
        /// <param name="influencedPropertyName"></param>
        /// <returns></returns>
        private static List<string> getPtyNames(string influencedPropertyName)
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    List<string> propertyNames = context.UnitConstraints
                        .Where(s => s.InfluencedPropertyName == influencedPropertyName)
                        .Select(s => s.PropertyName)
                        .ToList();
                    return propertyNames;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }


        public static int DeleteAll()
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    var unitModels = context.UnitModels;
                    foreach (var unitModel in unitModels)
                    {
                        context.UnitModels.Remove(unitModel);
                    }
                    return context.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 从excel导入
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static int InsertFromExcel(DataTable dataTable)
        {
            using (var context = new AnnonContext())
            {
                try
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var unitModel = new UnitModel
                        {
                            Property = dataRow["Property"].ToString(),
                            Value = dataRow["Value"].ToString(),
                            ValueDescription = dataRow["ValueDescription"].ToString(),
                            Condition = dataRow["Condition"].ToString(),
                        };
                        context.UnitModels.Add(unitModel);
                    }
                    return context.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }

        }



       



    }
}