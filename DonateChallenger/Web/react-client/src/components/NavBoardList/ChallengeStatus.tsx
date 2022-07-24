import { observer } from 'mobx-react';
import React from 'react';
import ChallengesBoardStore from '../../stores/containers/ChallengesBoardStore';
import { useInjection } from '../../utilities/ioc/ioc.react';
import iocStores from '../../utilities/ioc/iocStores';

interface ChallengeStatusProps {
     title: string,
     onClick: React.MouseEventHandler<HTMLSpanElement>
}

const ChallengeStatus = observer((props: ChallengeStatusProps) => {

     const store = useInjection<ChallengesBoardStore>(iocStores.challengesBoardStore);
     const className = props.title !== store.currentChallengeStatus ? 'fs-4 color-silver' : 'fs-3 color-white';

     return (
          <div role='button' className={className} onClick={props.onClick}>{props.title}</div>
     );
});

export default ChallengeStatus;