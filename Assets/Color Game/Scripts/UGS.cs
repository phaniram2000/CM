using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalScripts 
{
    public class UGS : MonoBehaviour
    {
        public static int totalMoney ;

        private void Awake()
        {
           //totalMoney = GameEssentials.instance.sd.GetTotalMoney();
			totalMoney = 0;
		}

        public static void AddMoneyToTotalMoney(bool plus, int val)
        {
            if (plus)
                totalMoney += val;
            else
                totalMoney -= val;

            //GameEssentials.instance.sd.SetTotalMoney(totalMoney);
        }
        
        
        public static void Log(object message)
        {
            print(message);
        }
    }
}

