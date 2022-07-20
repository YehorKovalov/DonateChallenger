import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { Comment } from "../../models/Comment";
import { PaginatedComments } from "../../models/PaginatedComments";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { CommentService } from "../../services/CommentService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class CommentsStore {

     @inject(iocServices.commentService) private readonly commentService!: CommentService
     @inject(iocStores.authStore) private readonly authStore!: AuthStore

     constructor() {
          makeAutoObservable(this);
     }

     paginatedComments: PaginatedComments<Comment> | null = null;

     public getChallengeComments = async (currentPage: number, commentsPerPage: number, challengeId: number) => {

          const paginatedComments = await this.commentService.getPaginatedComments(currentPage, commentsPerPage, challengeId);
          this.paginatedComments = paginatedComments
     }

     public getUserCommentsForChallenge = async (currentPage: number, commentsPerPage: number, challengeId: number, userId: string) => {

          if (!this.authStore.user || !(this.authStore.userRole == UserRole.Manager)) {
               alert("You don't have an access")
               return;
          }

          const paginatedComments = await this.commentService.getUserPaginatedComments(currentPage, commentsPerPage, challengeId, userId);
          this.paginatedComments = paginatedComments;
     }
}