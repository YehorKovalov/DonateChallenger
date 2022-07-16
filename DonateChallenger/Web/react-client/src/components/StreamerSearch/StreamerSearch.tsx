import { observer } from "mobx-react";
import { useEffect, useState } from "react";
import { Container, Row } from "react-bootstrap";
import StreamersStore from "../../stores/states/StreamersStore";
import { useInjection } from "../../utilities/ioc/ioc.react";
import iocStores from "../../utilities/ioc/iocStores";
import MovableInput from "../MovableInput";
import SearchIcon from "../SearchIcon";
import './styles.css';

const StreamerSearch = observer(() => {

     const streamersStore = useInjection<StreamersStore>(iocStores.streamersStore);
     const [showSearchedStreamers, setShowSearchedStreamers] = useState(false);

     useEffect(() => {
          const handleSearchingStreamersByNickname = async () => { await streamersStore.searchStreamersByNickname(); }
          handleSearchingStreamersByNickname();
     }, [streamersStore.nicknameForSearching]);

     const handleBluring = () => {
          setTimeout(() => { setShowSearchedStreamers(false) }, 100);
     }

     return (
          <div className="order_search text-center mt-2 mb-2">
               <MovableInput placeholder="Streamer..." onBlur={handleBluring} onFocus={() => setShowSearchedStreamers(true)}
                    value={streamersStore.nicknameForSearching} onChange={e => streamersStore.nicknameForSearching = e.target.value}
                    className="me-3"/>
               <SearchIcon/>
               {showSearchedStreamers &&
                    <Container className="nicknames_list">
                         {streamersStore.searchedStreamersByNickname.map(s =>
                              <div className="hover_white order_search__nickname"
                                   role='button' key={s.streamerId} onClick={() => streamersStore.selectStreamer(s)}
                              >
                                        {s.streamerNickname}
                              </div>)
                         }
                    </Container>
               }
               
               {streamersStore.selectedStreamer.streamerId &&
                    <Row className="fs-2 pt-3">
                         <div className="streamer-parameter_text">
                              <span>Streamer:</span>
                              <span className="ms-4 border-bottom">{streamersStore.selectedStreamer.streamerNickname}</span>
                         </div>
                         <div className="streamer-parameter_text">
                              <span>Has minimal donation price:</span>
                              <span className="ms-4 border-bottom">{streamersStore.selectedStreamer.minDonatePrice}</span>
                         </div>
                    </Row>
               }
          </div>
     );
});

export default StreamerSearch;