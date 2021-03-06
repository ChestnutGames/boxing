﻿using System;
using System.Collections;
//using UnityEngine; 
using System.Reflection; 

namespace SPSGame
{
    /// <summary>
    /// 游戏Action处理工厂
    /// </summary>
    public abstract class ActionFactory
    {
        public System.Reflection.Assembly ActionAssembly { set; get; }

        protected Hashtable lookupType = new Hashtable();
        protected virtual string ActionFormat()
        {
            return "Action_{0}";
        }

        public ActionBase CreateAction(object actionType)
        {
            ActionBase action = null;
            try
            {
                string name = string.Format(ActionFormat(), actionType);
                var type = (Type)lookupType[name];
                lock (lookupType)
                {
                    if (type == null)
                    {
                        if (null != ActionAssembly)
                        {
                            type = ActionAssembly.GetType(name);
                            if (null != type)
                                lookupType[name] = type;
                        }
                    }
                }
                if (type != null)
                {
                    //action.ActionId = name;
                    //action = Activator.CreateInstance(type) as ActionBase;
                    //action = FastActivator.Create(type) as ActionBase;
                }
            }
            catch (Exception ex)
            {
                DebugMod.LogError("action create error:" + ex);
            }
            return action;
        }
    }

}
