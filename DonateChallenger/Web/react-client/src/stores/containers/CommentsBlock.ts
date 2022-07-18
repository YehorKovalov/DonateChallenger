import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import CommentsConstants from "../../utilities/CommentsConstants";
import iocStores from "../../utilities/ioc/iocStores";
import { formPages } from "../../utilities/PagesProvider";
import CommentPagination from "../components/CommentPagination";
import CommentsStore from "../states/CommentsStore";

@injectable()
export default class ChallengesBoardStore {

     @inject(iocStores.commentsStore) private readonly commentsStore!: CommentsStore;
     @inject(iocStores.commentPagination) private readonly pagination!: CommentPagination;
     private readonly commentsPerPage: number = CommentsConstants.COMMENTS_PER_PAGE;

     constructor() {
          makeAutoObservable(this);
     }


     public getChallenges = async (challengeId: number) => {

          await this.commentsStore.getChallengeComments(this.pagination.currentPage, this.commentsPerPage, challengeId);
          
          const totalPages = this.commentsStore.paginatedComments!.totalPages;

          this.pagination.pagesAmount = totalPages;
          this.pagination.buttons = formPages(totalPages);
     }
}