import { observer } from 'mobx-react';
import SkippedChallengeCard from '../../components/SkippedChallengeCard';
import ChallengesStore from '../../stores/states/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const SkippedChallengesBoard = observer(() => {

     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);

     return (
          <div className='d-flex flex-wrap justify-content-evenly'>
               {challengesStore.paginatedChallenges?.data.map(c => <SkippedChallengeCard
                         key={c.challengeId} challengeId={c.challengeId}
                         description={c.description} donateFrom={c.donateFrom}
                         donatePrice={c.donatePrice} createdTime={c.createdTime}/>
                    )}
          </div>
     );
});

export default SkippedChallengesBoard;