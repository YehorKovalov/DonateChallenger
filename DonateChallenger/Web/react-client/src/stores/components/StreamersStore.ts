import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { SearchedStreamerByNickname } from "../../models/SearchedStreamerByNickname";
import { StreamerService } from "../../services/StreamerService";
import iocServices from "../../utilities/ioc/iocServices";

@injectable()
export default class StreamersStore {
     @inject(iocServices.streamerService) private readonly streamerService!: StreamerService;

     constructor() {
          makeAutoObservable(this);
     }

     searchedStreamersByNickname: SearchedStreamerByNickname[] = [];
     nicknameForSearching: string = '';
     selectedStreamer: SearchedStreamerByNickname = {
          streamerId: '',
          streamerNickname: '',
          merchantId: '',
          minDonatePrice: 0
     }

     public searchStreamersByNickname = async () => {
          if (this.nicknameForSearching){
               const result = await this.streamerService.searchStreamersByNickname(this.nicknameForSearching);
               this.searchedStreamersByNickname = result.data;
          }
          else {
               this.searchedStreamersByNickname = []
          }
     }

     public selectStreamer = (streamer: SearchedStreamerByNickname) => {
          if (streamer === this.selectedStreamer) {
               return;
          }

          this.selectedStreamer = streamer;
          this.nicknameForSearching = this.selectedStreamer.streamerNickname;
     }
}