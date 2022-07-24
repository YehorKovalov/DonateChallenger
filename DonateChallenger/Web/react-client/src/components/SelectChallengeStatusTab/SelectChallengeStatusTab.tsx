import { observer } from 'mobx-react-lite';
import { Tab, Tabs } from 'react-bootstrap';
import CompletedChallengesList from '../../containers/CompletedChallengesList';
import CurrentChallengesList from '../../containers/CurrentChallengesList';
import SkippedChallengesList from '../../containers/SkippedChallengesList';
import { ChallengeStatusEnum } from '../../models/ChallengeStatusEnum';
import CatalogChallengeManagerStore from '../../stores/containers/CatalogChallengeManagerStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

interface SelectChallengeStatusTabProps {
     streamerId: string;
}

const SelectChallengeStatusTab = observer((props: SelectChallengeStatusTabProps) => {
     const catalogChallengesManager = useInjection<CatalogChallengeManagerStore>(iocStores.catalogChallengeManagerStore);

     const handleOnSelect = async (tabName: string | null) => {
          switch (tabName) {
               case "current":
                    catalogChallengesManager.selectedStatus = ChallengeStatusEnum.Current;
                    await catalogChallengesManager.getPaginatedCurrentChallenges(props.streamerId);
                    break;
               case "completed":
                    catalogChallengesManager.selectedStatus = ChallengeStatusEnum.Completed;
                    await catalogChallengesManager.getPaginatedCompletedChallenges(props.streamerId);
                    break;
               case "skipped":
                    catalogChallengesManager.selectedStatus = ChallengeStatusEnum.Skipped;
                    await catalogChallengesManager.getPaginatedSkippedChallenges(props.streamerId);
                    break;
          }
     }

     return (
          <Tabs className="mb-5 pt-5" onSelect={handleOnSelect}>
                    <Tab eventKey="current" title="Current">
                         <CurrentChallengesList streamerId={props.streamerId}/>
                    </Tab>
                    <Tab eventKey="completed" title="Completed">
                         <CompletedChallengesList streamerId={props.streamerId}/>
                    </Tab>
                    <Tab eventKey="skipped" title="Skipped">
                         <SkippedChallengesList streamerId={props.streamerId}/>
                    </Tab>
          </Tabs>
     );
});

export default SelectChallengeStatusTab;