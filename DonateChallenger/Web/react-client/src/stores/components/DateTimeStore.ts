import { injectable } from "inversify";
import moment from "moment";

@injectable()
export default class DateTimeStore {
     public getUserFriendlyDateTime = (time: string): string => { return moment(new Date(time), "YYYYMMDD").fromNow(); }
}