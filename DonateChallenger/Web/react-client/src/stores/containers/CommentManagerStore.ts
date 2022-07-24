import { inject, injectable } from "inversify";
import { makeAutoObservable } from "mobx";
import { Comment } from "../../models/Comment";
import { CommentToAdd } from "../../models/CommentToAdd";
import { PaginatedComments } from "../../models/PaginatedComments";
import { UserRole } from "../../models/UserRole";
import AuthStore from "../../oidc/AuthStore";
import { CommentManagerService } from "../../services/CommentManagerService";
import iocServices from "../../utilities/ioc/iocServices";
import iocStores from "../../utilities/ioc/iocStores";

@injectable()
export default class CommentManagerStore {

     @inject(iocServices.commentManagerService) private readonly managerService!: CommentManagerService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private commentPerPage = 20;
     constructor() {
          makeAutoObservable(this);
     }

     comment: Comment = {
          commentId: 0,
          message: '',
          challengeId: 0,
          date: '',
          username: '',
          userId: ''
     }

     commentToAdd: CommentToAdd = {
          message: '',
          challengeId: 0,
          userId: ''
     }

     paginatedComments: PaginatedComments<Comment> = {
          totalCount: 0,
          totalPages: 0,
          commentsPerPage: this.commentPerPage,
          currentPage: 0,
          data: []
     }

     public add = async () => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.addCommentToChallenge(this.comment.message, this.comment.challengeId, this.comment.userId);
               if (!result.data) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }

               alert("Successful");
          }
     }

     public getPaginated = async (challengeId: number) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.getPaginatedComments(this.paginatedComments.currentPage, this.paginatedComments.commentsPerPage, challengeId);
               this.paginatedComments = result;
          }
     }

     public getById = async (commentId: number) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.getCommentByCommentId(commentId);
               if (!result.data) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }

               this.comment = result.data;
          }
     }

     public update = async (commentId: number) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const comment = this.paginatedComments.data.find(f => f.commentId === commentId);
               if (!comment) {
                    alert("Something went wrong, check logs or reload page");
                    return;
               }
     
               const result = await this.managerService.updateComment(comment.commentId, comment.message, comment.challengeId, comment.userId);
               if (result.data && result.data === comment.commentId) {
                    alert("Successful");
                    return;
               }
     
               alert("Not updated, check logs");          
          }
     }

     public delete = async (commentId: number) => {
          if (this.authStore.user && this.authStore.userRole === UserRole.Manager) {
               const result = await this.managerService.deleteComment(commentId);
               if (result.data) {
                    alert("Successful");
                    return;
               }
     
               alert("Not deleted, check logs");
          }
     }
}