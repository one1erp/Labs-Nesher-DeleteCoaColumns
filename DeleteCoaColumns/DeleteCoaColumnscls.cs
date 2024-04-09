using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Common;
using DAL;
using LSEXT;
using LSSERVICEPROVIDERLib;

namespace DeleteCoaColumns
{


    [ComVisible(true)]
    [ProgId("DeleteCoaColumns.DeleteCoaColumnscls")]
    public class DeleteCoaColumnscls : IEntityExtension
    {
        private INautilusServiceProvider sp;

        public ExecuteExtension CanExecute(ref IExtensionParameters Parameters)
        {
            return ExecuteExtension.exEnabled;
        }

        public void Execute(ref LSExtensionParameters Parameters)
        {
            sp = Parameters["SERVICE_PROVIDER"];
            var records = Parameters["RECORDS"];
            var id = records.Fields["U_COA_REPORT_ID"].Value;
            var ntlsCon = Utils.GetNtlsCon(sp);
            Utils.CreateConstring(ntlsCon);
            long lid = long.Parse(id.ToString());





            var ff = new FieldsForm(lid);
            if (!ff.IsDisposed)
                ff.ShowDialog();





        }
    }
}
