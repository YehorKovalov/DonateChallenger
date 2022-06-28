import { observer } from 'mobx-react';
import CurrentChallengeCard from '../../components/CurrentChallengeCard';
import ChallengesStore from '../../stores/components/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

const CurrentChallengesBoard = observer(() => {

     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);

     return (
          <div className='d-flex flex-wrap justify-content-evenly'>
               {challengesStore.paginatedChallenges?.data.map(c => <CurrentChallengeCard
                         key={c.challengeId} challengeId={c.challengeId}
                         description={c.description} donateFrom={c.donateFrom}
                         donatePrice={c.donatePrice} createdTime={c.createdTime}/>
                    )}
          </div>
     );
});

export default CurrentChallengesBoard;