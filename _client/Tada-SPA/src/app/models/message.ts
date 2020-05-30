export interface Message {
    id: number;
    content: string;
    isRead: boolean;
    sentDate: string;
    readDate: string | null;
    senderId: number;
    receiverId: number;
    senderDeleted: boolean;
    receiverDeleted: boolean;
    senderKnownAs: string;
    senderPhotoUrl: string;
    receiverKnownAs: string;
    receiverPhotoUrl: string;
}
