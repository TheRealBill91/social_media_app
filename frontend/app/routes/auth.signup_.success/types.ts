interface ActionSuccessReponse {
  success: true;
}

interface ActionErrorResponse {
  success: false;
  error: string;
}

export type ActionResponse = ActionSuccessReponse | ActionErrorResponse;
