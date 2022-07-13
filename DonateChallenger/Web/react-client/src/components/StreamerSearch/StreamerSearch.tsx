import { observer } from "mobx-react";
import { useState } from "react";
import { Container } from "react-bootstrap";
import StreamersStore from "../../stores/components/StreamersStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import MovableInput from "../MovableInput";
import SearchIcon from "../SearchIcon";
import './styles.css';

const StreamerSearch = observer(() => {
     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);

     const [showSearchedStreamers, setShowSearchedStreamers] = useState(false);

     const handleBluring = () => {
          setTimeout(() => {
               setShowSearchedStreamers(false)
          }, 100);
     }
     return (
          <div className="order_search">
               <MovableInput onBlur={handleBluring} onFocus={() => setShowSearchedStreamers(true)}
                    value={streamersStore.nicknameForSearching} onChange={e => streamersStore.nicknameForSearching = e.target.value} className="me-3"/>
               <SearchIcon/>
               {showSearchedStreamers &&
                    <Container className="nicknames_list">
                         {streamersStore.searchedStreamersByNickname.map(s =>
                              <div className="hover_white order_search__nickname"
                                   role='button' key={s.streamerId} onClick={() =>
                                        {
                                             streamersStore.selectStreamer(s)}
                                        }>
                                        {s.streamerNickname}
                              </div>)}
                    </Container>
               }
          </div>
     );
});

export default StreamerSearch;