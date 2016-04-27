﻿using Chloe.DbExpressions;
using Chloe.Descriptors;
using Chloe.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chloe.Query.Visitors
{
    public class GeneralExpressionVisitor1 : ExpressionVisitorBase
    {
        MappingTypeDescriptor _typeDescriptor;

        public GeneralExpressionVisitor1(MappingTypeDescriptor typeDescriptor)
        {
            this._typeDescriptor = typeDescriptor;
        }

        protected override DbExpression VisitMemberAccess(MemberExpression exp)
        {
            ParameterExpression p;
            if (ExpressionExtensions.IsDerivedFromParameter(exp, out p))
            {
                Stack<MemberExpression> reversedExps = ExpressionExtensions.Reverse(exp);

                Dictionary<MemberInfo, DbColumnAccessExpression> memberColumnMap = this._typeDescriptor.MemberColumnMap;

                DbExpression dbExp = null;
                bool first = true;
                foreach (var me in reversedExps)
                {
                    if (first)
                    {
                        DbColumnAccessExpression dbColumnAccessExpression;
                        if (!memberColumnMap.TryGetValue(me.Member, out dbColumnAccessExpression))
                        {
                            throw new Exception(string.Format("成员 {0} 未映射任何列", me.Member.Name));
                        }

                        dbExp = dbColumnAccessExpression;
                        first = false;
                    }
                    else
                    {
                        DbMemberExpression dbMe = new DbMemberExpression(me.Member, dbExp);
                        dbExp = dbMe;
                    }
                }

                if (dbExp != null)
                {
                    return dbExp;
                }
                else
                    throw new Exception();
            }
            else
            {
                return base.VisitMemberAccess(exp);
            }
        }

        protected override DbExpression VisitParameter(ParameterExpression exp)
        {
            throw new NotSupportedException(exp.ToString());
        }

    }
}
