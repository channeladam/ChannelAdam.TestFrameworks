//-----------------------------------------------------------------------
// <copyright file="BizTalkExplorer.cs">
//     Copyright (c) 2016 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

namespace ChannelAdam.TestFramework.BizTalk
{
    using System.Linq;

    using Microsoft.BizTalk.ExplorerOM;
    using System;

    public static class BizTalkExplorer
    {
        public static BtsCatalogExplorer CreateBizTalkCatalogExplorer(string managementDatabaseConnectionString)
        {
            var explorer = new BtsCatalogExplorer
            {
                ConnectionString = managementDatabaseConnectionString
            };

            return explorer;
        }

        public static Application FindBizTalkApplication(string applicationName, BtsCatalogExplorer explorer)
        {
            if (explorer == null) throw new ArgumentNullException(nameof(explorer));

            return explorer.Applications[applicationName];
        }

        public static Application FindBizTalkApplicationContainingOrchestration(string orchestrationFullName, BtsCatalogExplorer explorer)
        {
            if (explorer == null) throw new ArgumentNullException(nameof(explorer));

            return explorer.Applications.Cast<Application>().SingleOrDefault(app => app.Orchestrations.Cast<BtsOrchestration>().Any(orch => orch.FullName == orchestrationFullName));
        }
    }
}
