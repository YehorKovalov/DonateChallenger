export interface PortionedProfiles<TProfile> {
     currentPortion: number;
     usersPerPortion: number;
     morePortionsExist: boolean;
     users: TProfile[];
}