import { inject, injectable } from "inversify";
import { CommentDto } from "../dtos/DTOs/CommentDto";
import { AddCommentRequest } from "../dtos/requests/AddCommentRequest";
import { GetPaginatedCommentsRequest } from "../dtos/requests/GetPaginatedCommentsRequest";
import { AddCommentResponse } from "../dtos/response/AddCommentResponse";
import { GetPaginatedCommentsResponse } from "../dtos/response/GetPaginatedCommentsResponse";
import AuthStore from "../oidc/AuthStore";
import iocServices from "../utilities/ioc/iocServices";
import iocStores from "../utilities/ioc/iocStores";
import { HttpService, MethodType } from "./HttpService";

export interface CommentService {
     addCommentToChallenge(message: string, challengeId: number, userId: string): Promise<AddCommentResponse<number>>;
     getPaginatedComments(currentPage: number, commentsPerPage: number, challengeId: number): Promise<GetPaginatedCommentsResponse<CommentDto>>;
     getUserPaginatedComments(currentPage: number, commentsPerPage: number, challengeId: number, userId: string): Promise<GetPaginatedCommentsResponse<CommentDto>>;
}

@injectable()
export default class DefaultCommentService implements CommentService {
     @inject(iocStores.authStore) private readonly authStore!: AuthStore;
     @inject(iocServices.httpService) private readonly httpService!: HttpService;

     private readonly CommentApiRoute = process.env.REACT_APP_COMMENT_CONTROLLER_ROUTE;
     private readonly CommentManagerApiRoute = process.env.REACT_APP_COMMENT_MANAGER_CONTROLLER_ROUTE;

     public async addCommentToChallenge(message: string, challengeId: number, userId: string): Promise<AddCommentResponse<number>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const url = `${this.CommentApiRoute}/add`;
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
          const url = `${this.CommentApiRoute}/paginatedcomments`;

          const request: GetPaginatedCommentsRequest = {
               currentPage: currentPage,
               challengeId: challengeId,
               commentsPerPage: commentsPerPage,
          };

          const response = await this.httpService.send<GetPaginatedCommentsResponse<CommentDto>>(url, method, headers, request);

          return { ...response.data };
     }

     public async getUserPaginatedComments(currentPage: number, commentsPerPage: number, challengeId: number, userId: string): Promise<GetPaginatedCommentsResponse<CommentDto>> {
          const headers = await this.authStore.tryGetAuthorizedHeaders();
          const method = MethodType.POST;
          const url = `${this.CommentApiRoute}/userpaginatedcomments`;

          const request: GetPaginatedCommentsRequest = {
               currentPage: currentPage,
               challengeId: challengeId,
               commentsPerPage: commentsPerPage,
               userId: userId
          };

          const response = await this.httpService.send<GetPaginatedCommentsResponse<CommentDto>>(url, method, headers, request);

          return { ...response.data };
     }
}