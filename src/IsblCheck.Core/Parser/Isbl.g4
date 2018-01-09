grammar Isbl;

@parser::header {#pragma warning disable 3021}
@lexer::header {#pragma warning disable 3021}

/*******************************************************************************
*********************************������� �������********************************
*******************************************************************************/
// ���������� ������������������� �������.
fragment A:('a'|'A');
fragment B:('b'|'B');
fragment C:('c'|'C');
fragment D:('d'|'D');
fragment E:('e'|'E');
fragment F:('f'|'F');
fragment G:('g'|'G');
fragment H:('h'|'H');
fragment I:('i'|'I');
fragment J:('j'|'J');
fragment K:('k'|'K');
fragment L:('l'|'L');
fragment M:('m'|'M');
fragment N:('n'|'N');
fragment O:('o'|'O');
fragment P:('p'|'P');
fragment Q:('q'|'Q');
fragment R:('r'|'R');
fragment S:('s'|'S');
fragment T:('t'|'T');
fragment U:('u'|'U');
fragment V:('v'|'V');
fragment W:('w'|'W');
fragment X:('x'|'X');
fragment Y:('y'|'Y');
fragment Z:('z'|'Z');

// ������� ������������������� �������.
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');
fragment �:('�'|'�');

// �����������.
WS: [ \t\r\n]+->skip;

// �����������.
BLOCK_COMMENT: '/*' .*? '*/'->skip;
LINE_COMMENT: '//' ~[\r\n]*->skip;

// �������� �����.
AND: A N D | �;
ELSE: E L S E | � � � � �;
ENDEXCEPT: E N D E X C E P T;
ENDFINALLY: E N D F I N A L L Y;
ENDFOREACH: E N D F O R E A C H | � � � � � � � �;
ENDIF: E N D I F | � � � � � � � � �;
ENDWHILE: E N D W H I L E | � � � � � � � � �;
EXCEPT: E X C E P T;
EXITFOR: E X I T F O R;
FINALLY: F I N A L L Y;
FOREACH: F O R E A C H | � � �;
IF: I F | � � � �;
IN: I N | �;
NOT: N O T | � �;
OR: O R | � � �;
TRY: T R Y;
WHILE: W H I L E | � � � �;

// ���������.
EQ: '=';
NEQ: '<>';
GT: '>';
GEQ: '>=';
LT: '<';
LEQ: '<=';
STR_EQ: '==';
STR_NEQ: '<<>>';
STR_GT: '>>';
STR_GEQ: '>>=';
STR_LT: '<<';
STR_LEQ: '<<=';
PLUS: '+';
MINUS: '-';
MUL: '*';
DIV: '/';
POINTER:'^';
CONCAT: '&';
L_PAREN: '(';
R_PAREN: ')';
L_BRACKET: '[';
R_BRACKET: ']';
COLON : ':';
COMMA: ';';
DOT: '.';

// ��������.
STRINGLITERAL: '"' ~["]* '"' | '\'' ~[\']* '\'';
NUMBERLITERAL: [0-9]+ ([\.] [0-9]+ )?;
IDENTLITERAL : ('a'..'z' | 'A'..'Z' | '�'..'�' | '�' | '�'..'�' | '�' | '!') ('a'..'z' | 'A'..'Z' | '�'..'�' | '�' | '�'..'�' | '�' | '0'..'9'| '_')*;

/*******************************************************************************
*********************************������� �������********************************
*******************************************************************************/
// ���� �����������.
statementBlock
    : ( declareVariableStatement 
      | assignStatement
      | invokeStatement
      | ifStatement
      | foreachStatement
      | whileStatement
      | tryStatement 
      | exitforStatement
      )*
    ;

// ���������� ����������.
declareVariableStatement
    : variable COLON typedefinition
    ;
// �����������.
assignStatement
    : ( variable COLON typedefinition
      | variable indexer? (DOT invocationCall)*
      | function (DOT invocationCall)+
      ) EQ expression
    ;
// ����� ������.
invokeStatement
    : variable indexer? (DOT invocationCall)+
    | function (DOT invocationCall)*
    ;
// �������.
ifStatement
    : IF expression statementBlock (ELSE statementBlock)? ENDIF
    ;
// ���������� ����.
foreachStatement
    : FOREACH variable (IN | EQ) expression statementBlock ENDFOREACH
    ;
// ���� � ������������.
whileStatement
    : WHILE expression statementBlock ENDWHILE
    ;
// ��������� ����������.
tryStatement
    : TRY statementBlock ( EXCEPT statementBlock ENDEXCEPT 
                         | FINALLY statementBlock ENDFINALLY
                         )
    ;
// ����� �� �����.
exitforStatement
    : EXITFOR
    ;
// ���������.
expression
    : operand
    | (PLUS | MINUS) expression
    | NOT expression
    | expression (MUL | DIV) expression
    | expression (PLUS | MINUS | CONCAT) expression
    | expression (EQ | NEQ | GT | GEQ | LT | LEQ | STR_EQ | STR_NEQ | STR_GT | STR_GEQ | STR_LT | STR_LEQ) expression
    | expression AND expression
    | expression OR expression
    ;
// �������.
operand
    : unsignedNumber
    | string
    | variable indexer? (DOT invocationCall)*
    | variable POINTER
    | function (DOT invocationCall)*
    | L_PAREN expression R_PAREN;
// �������.
function
    : identifier L_PAREN parameterList R_PAREN
    ;
// COM �����.
invocationCall
    : identifier (L_PAREN parameterList R_PAREN)?
    ;
// ����������.
indexer
    : L_BRACKET parameterList R_BRACKET
    ;
// ���������.
typedefinition
    : type (DOT subtype)?
    ;
// ������ ����������.
parameterList
    : expression? (COMMA expression?)*
    ;
// �������������. ���� ������� IN, �������� �, ��� �����.
identifier
    : IDENTLITERAL
    | IN
    ;
// ����������.
variable
    : IDENTLITERAL
    ;
// ���.
type
    : IDENTLITERAL
    ;
// ���-���.
subtype
    : IDENTLITERAL
    ;
// ���������� �����.
unsignedNumber
    : NUMBERLITERAL
    ;
// ������.
string
    : STRINGLITERAL
    ;