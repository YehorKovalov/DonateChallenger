import { observer } from 'mobx-react';
import CompletedChallengeCard from '../../components/CompletedChallengeCard';
import ChallengesStore from '../../stores/states/ChallengesStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';
import CommentsBlock from '../CommentsBlock/CommentsBlock';

const CompletedChallengesBoard = observer(() => {

     const challengesStore = useInjection<ChallengesStore>(iocStores.challengesStore);

     return (
          <div className='d-flex flex-wrap justify-content-evenly'>
               {challengesStore.paginatedChallenges?.data.map(c => <CompletedChallengeCard
                         key={c.challengeId} challengeId={c.challengeId}
                         description={c.description} donateFrom={c.donateFrom}
                         donatePrice={c.donatePrice} createdTime={c.createdTime}/>
                    )}
               <CommentsBlock/>
          </div>
     );
});

export default CompletedChallengesBoard;