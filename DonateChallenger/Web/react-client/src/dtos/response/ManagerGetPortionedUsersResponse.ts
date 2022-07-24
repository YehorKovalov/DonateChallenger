export interface ManagerGetPortionedUsersResponse<TUser> {
     currentPortion: number;
     usersPerPortion: number;
     morePortionsExist: boolean;
     users: TUser[];
}