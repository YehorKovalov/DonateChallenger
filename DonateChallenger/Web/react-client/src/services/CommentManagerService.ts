import { inject, injectable } from "inversify";
import { CommentDto } from "../dtos/DTOs/CommentDto";
import { AddCommentRequest } from "../dtos/requests/AddCommentRequest";
import { GetPaginatedCommentsRequest } from "../dtos/requests/GetPaginatedCommentsRequest";
import { UpdateCommentRequest } from "../dtos/requests/UpdateCommentRequest";
import { AddCommentResponse } from "../dtos/response/AddCommentResponse";
import { DeleteCommentResponse } from "../dtos/response/DeleteCommentResponse";
import { GetCommentByCommentIdResponse } from "../dtos/response/GetCommentByCommentIdResponse";
import { GetPaginatedCommentsResponse } from "../dtos/response/GetPaginatedCommentsResponse";
import { UpdateCommentResponse } from "../dtos/response/UpdateCommentResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface CommentManagerService {
     addCommentToChallenge(message: string, challengeId: number, userId: string): Promise<AddCommentResponse<number>>;
     deleteComment(commentId: number): Promise<DeleteCommentResponse<boolean>>;
     updateComment(commentId: number, message: string, challengeId: number, userId: string): Promise<UpdateCommentResponse<number>>;
     getPaginatedComments(currentPage: number, commentsPerPage: number, challengeId: number): Promise<GetPaginatedCommentsResponse<CommentDto>>;
     getCommentByCommentId(commentId: number): Promise<GetCommentByCommentIdResponse<CommentDto>>;
}

@injectable()
export default class DefaultCommentManagerService implements CommentManagerService {

     @inject(iocServices.httpService) private readonly httpService!: HttpService;
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;

     private readonly commentManagerApiRoute = process.env.REACT_APP_COMMENT_MANAGER_CONTROLLER_ROUTE;

     public async addCommentToChallenge(message: string, challengeId: number, userId: string): Promise<AddCommentResponse<number>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.commentManagerApiRoute}/add`;
          const request: AddCommentRequest = {
               message: message,
               challengeId: challengeId,
               userId: userId
          }

          var response = await this.httpService.send<AddCommentResponse<number>>(url, MethodType.POST, headers, request);
          return { ...response.data };
     }

     public async getPaginatedComments(currentPage: number, commentsPerPage: number, challengeId: number): Promise<GetPaginatedCommentsResponse<CommentDto>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const method = MethodType.POST;
          const url = `${this.commentManagerApiRoute}/paginatedcomments`;

          const request: GetPaginatedCommentsRequest = {
               currentPage: currentPage,
               challengeId: challengeId,
               commentsPerPage: commentsPerPage,
          };

          const response = await this.httpService.send<GetPaginatedCommentsResponse<CommentDto>>(url, method, headers, request);

          return { ...response.data };
     }

     public async deleteComment(commentId: number): Promise<DeleteCommentResponse<boolean>> {

          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.commentManagerApiRoute}/delete?commentId=${commentId}`;

          var response = await this.httpService.send<DeleteCommentResponse<boolean>>(url, MethodType.GET, headers);

          return { ...response.data };
     }

     public async updateComment(commentId: number, message: string, challengeId: number, userId: string): Promise<UpdateCommentResponse<number>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.commentManagerApiRoute}/update`;
          const request: UpdateCommentRequest = {
               commentId: commentId,
               message: message,
               challengeId: challengeId,
               userId: userId
          }

          var response = await this.httpService.send<UpdateCommentResponse<number>>(url, MethodType.POST, headers, request);
          return { ...response.data };
     }

     public async getCommentByCommentId(commentId: number): Promise<GetCommentByCommentIdResponse<CommentDto>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.commentManagerApiRoute}/get?commentId=${commentId}`;

          var response = await this.httpService.send<GetCommentByCommentIdResponse<CommentDto>>(url, MethodType.GET, headers);

          return { ...response.data };
     }
}