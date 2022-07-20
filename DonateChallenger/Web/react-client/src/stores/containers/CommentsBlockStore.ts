import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import AuthStore from "../../oidc/AuthStore";
import { CommentService } from "../../services/CommentService";
import CommentsConstants from "../../utilities/CommentsConstants";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";
import { formPages } from "../../utilities/PagesProvider";
import CommentPaginationStore from "../components/CommentPaginationStore";
import CommentsStore from "../states/CommentsStore";
import CommentStore from "../states/CommentStore";

@injectable()
export default class CommentsBlockStore {
     
     @inject(iocStores.commentsStore) private readonly commentsStore!: CommentsStore;
     @inject(iocStores.commentPaginationStore) private readonly pagination!: CommentPaginationStore;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore
     @inject(iocStores.commentStore) private readonly commentStore!: CommentStore;
     @inject(iocServices.commentService) private readonly commentService!: CommentService

     private readonly commentsPerPage: number = CommentsConstants.COMMENTS_PER_PAGE;

     constructor() {
          makeAutoObservable(this);
     }

     showBlock = false;

     public getComments = async (challengeId: number, notIngoreGettingCommentsIfChallengeIdDuplicated = false) => {

          if (notIngoreGettingCommentsIfChallengeIdDuplicated || challengeId != this.commentStore.currentChallengeId) {
               await this.commentsStore.getChallengeComments(this.pagination.currentPage, this.commentsPerPage, challengeId);
               
               const totalPages = this.commentsStore.paginatedComments!.totalPages;

               this.pagination.pagesAmount = totalPages;
               this.pagination.buttons = formPages(totalPages);
          }

          this.commentStore.currentChallengeId = challengeId;
     }

     public addComment = async () => {
          if (!this.authStore.user) {
               await this.authStore.tryGetUser();
          }

          if (this.authStore.user && this.authStore.user.profile.role) {

               this.commentStore.userId = this.authStore.user?.profile.sub;

               const userId = this.commentStore.userId;
               const message = this.commentStore.message;
               const challengeId = this.commentStore.currentChallengeId;
               
               console.log(`${message}; ${challengeId}; ${userId}`);

               const response = await this.commentService.addCommentToChallenge(message, challengeId, userId);
               if (response.data) {
                    console.log(response.data)
                    await this.getComments(challengeId, true);
                    this.commentStore.message = "";
               }
          }
          else {
               console.log("Unauthorized user")
          }
     }
}