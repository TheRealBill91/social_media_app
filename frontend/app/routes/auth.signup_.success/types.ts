interface ActionSuccessReponse {
  success: true;
  serverSuccessMessage: string;
}

interface ActionErrorResponse {
  success: false;
  error: string;
}

export type ActionResponse = ActionSuccessReponse | ActionErrorResponse;
